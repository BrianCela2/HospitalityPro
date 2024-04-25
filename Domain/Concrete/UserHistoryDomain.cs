using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationsDTOS;
using DTO.UserHistoryDTOs;
using Entities.Models;
using Helpers.StaticFunc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace Domain.Concrete
{
    internal class UserHistoryDomain : DomainBase, IUserHistoryDomain
    {
		private readonly PaginationHelper<UserHistory> _paginationHelper;

		public UserHistoryDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
			_paginationHelper = new PaginationHelper<UserHistory>();

		}

		private IUserHistoryRepository userHistoryRepository => _unitOfWork.GetRepository<IUserHistoryRepository>();

        public async Task<PaginatedUserHistoryDTO> GetHistory(int page, int pageSize, string sortField, string sortOrder)
        {
            IEnumerable<UserHistory> userHistory = userHistoryRepository.GetAll();
			IEnumerable<UserHistory> paginatedUserHistory = _paginationHelper.GetPaginatedData(userHistory, page, pageSize, sortField, sortOrder);
			var allUserHistory = _mapper.Map<IEnumerable<UserHistoryDTO>>(paginatedUserHistory);
			var totaluserHistoryCount = userHistory.Count();
			var totalPages = (int)Math.Ceiling((double)totaluserHistoryCount / pageSize);
			return new PaginatedUserHistoryDTO
			{
				UserHistory = allUserHistory,
				TotalPages = totalPages
			};
		}
	}
}