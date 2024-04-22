using DAL.Contracts;
using Entities.Models;

namespace DAL.Concrete
{
    internal class UserHistoryRepository : BaseRepository<UserHistory, Guid>, IUserHistoryRepository
    {
        public UserHistoryRepository(HospitalityProContext dbContext) : base(dbContext)
        {
        }
    }
}
