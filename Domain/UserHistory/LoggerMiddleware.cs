using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using DTO.UserHistoryDTOs;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.JWT;
using Helpers.StaticFunc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly bool _isRequestResponseLoggingEnabled;
    private readonly IMapper _mapper; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public LoggerMiddleware(RequestDelegate next, IConfiguration config,IMapper mapper,IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _isRequestResponseLoggingEnabled = config.GetSection("EnableRequestResponseLogging").Value == "true";
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }
    private IUserHistoryRepository UserHistoryRepository => _unitOfWork.GetRepository<IUserHistoryRepository>();
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (_isRequestResponseLoggingEnabled && LogRequest(httpContext))
        {
            var originalResponseBody = httpContext.Response.Body;
            using var newResponseBody = new MemoryStream();
            httpContext.Response.Body = newResponseBody;
            UserHistoryDTO userHistoryDTO = new UserHistoryDTO
            {
                LoginDate = DateTime.UtcNow.Date,
                Browser = httpContext.Request.Headers["User-Agent"],
            };

            
            await _next(httpContext);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            var userId = StaticFunc.GetUserId(_httpContextAccessor).ToString();
            switch (httpContext.Request.Path.Value)
            {
                case "/api/Reservation":
                    userHistoryDTO.UserAction = UserAction.AddReservation;
                    userHistoryDTO.Title = $"{userId} made a new reservation";
                    break;
                case "/api/Auth/login":
                    userHistoryDTO.UserAction = UserAction.Login;
                    var userIdToken = JWT.GetUserIdFromToken(responseBodyText);
                    userHistoryDTO.Title = !string.IsNullOrEmpty(userId) ? $"{userIdToken} logged in" : "Anonymous logged in";
                    break;
                case var path when path.StartsWith("/api/Reservation/status/"):
                    userHistoryDTO.UserAction = UserAction.StatusChange;
                    userHistoryDTO.Title = $"{userId} changed reservation status";
                    break;
                default:
                    userHistoryDTO.UserAction = UserAction.Other; 
                    userHistoryDTO.Title = $"{userId} made an action";
                    break;
            }
 
            AddToDatabase(userHistoryDTO, httpContext);
            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
        }
        else
        {
            await _next(httpContext);
        }
    }

    private static async Task<string> ReadBodyFromRequest(HttpRequest request)
    {
        request.EnableBuffering();

        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();

        request.Body.Position = 0;
        return requestBody;
    }
   
    private bool LogRequest(HttpContext context)
    {
        List<(string endpoint, string method)> allowedEndpoints = new List<(string, string)>
    {
        ("/api/Reservation", "POST"),
        ("/api/Auth/login", "POST"),
        ("/api/Reservation/status", "PUT")
    };

        foreach (var (endpointPath, method) in allowedEndpoints)
        {
            if (context.Request.Path.StartsWithSegments(endpointPath, StringComparison.OrdinalIgnoreCase))
            {
                if (context.Request.Method.Equals(method, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }            
            }
        }
        return false;
    }

    public void AddToDatabase(UserHistoryDTO userHistoryDTO, HttpContext context)
    {
        var userHistory = _mapper.Map<UserHistory>(userHistoryDTO);

        UserHistoryRepository.Add(userHistory);
        _unitOfWork.Save();

    }
}