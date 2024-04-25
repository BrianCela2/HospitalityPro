using DTO.UserHistoryDTOs;

namespace Domain.Contracts
{
    public interface IUserHistoryDomain
    {
		Task<PaginatedUserHistoryDTO> GetHistory(int page, int pageSize, string sortField, string sortOrder);

	}
}
