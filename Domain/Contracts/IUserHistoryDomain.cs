using DTO.UserHistoryDTOs;

namespace Domain.Contracts
{
    public interface IUserHistoryDomain
    {
        IEnumerable<UserHistoryDTO> GetHistory();
    }
}
