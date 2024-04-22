using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using DTO.UserHistoryDTOs;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.StaticFunc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

public class RequestResponseLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly bool _isRequestResponseLoggingEnabled;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RequestResponseLoggerMiddleware> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    public RequestResponseLoggerMiddleware(RequestDelegate next, IConfiguration config, IUnitOfWork unitOfWork, ILogger<RequestResponseLoggerMiddleware> logger, IHttpContextAccessor httpContextAccessor,IMapper mapper)
    {
        _next = next;
        _isRequestResponseLoggingEnabled = config.GetSection("EnableRequestResponseLogging").Value == "true";
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    private IUserHistoryRepository UserHistoryRepository => _unitOfWork.GetRepository<IUserHistoryRepository>();

    public async Task InvokeAsync(HttpContext context)
    {
        if (_isRequestResponseLoggingEnabled)
        {

            UserHistoryDTO userHistory = new UserHistoryDTO
            {
                LoginDate = DateTime.UtcNow.Date,
                Browser = context.Request.Headers["User-Agent"],
            };
            var userId = StaticFunc.GetUserId(_httpContextAccessor).ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                userHistory.Title = $"{userId} clicked {context.Request.Path.Value}";
            }
            else
            {
                userHistory.Title = $"Anonymous clicked {context.Request.Path.Value}";
            }
            switch (context.Request.Path.Value)
            {
                case "/addReservation":
                    userHistory.UserAction = UserAction.AddReservation;
                    break;
                case "/api/Auth/login":
                    userHistory.UserAction = UserAction.Login;
                    break;
                case var path when path.StartsWith("/api/Reservation/status/"):
                    userHistory.UserAction = UserAction.StatusChange;
                    break;
                default:
                    userHistory.UserAction = UserAction.Other;
                    break;
            }
            await _next(context);

            await AddToDatabase(userHistory);
        }
        else
        {
            await _next(context);
        }
    }


    public async Task AddToDatabase(UserHistoryDTO userHistoryDTO)
    {
        var userHistory = _mapper.Map<UserHistory>(userHistoryDTO);
        try
        {
            UserHistoryRepository.Add(userHistory);
             _unitOfWork.Save();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding user history to the database.");
            throw; 
        }
    }
}
