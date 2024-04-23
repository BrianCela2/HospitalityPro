using DAL.Contracts;
using Entities.Models;

namespace DAL.Concrete
{
    internal class UserHistoryRepository : BaseRepository<UserHistory, Guid>, IUserHistoryRepository
    {
        public UserHistoryRepository(HospitalityProContext dbContext) : base(dbContext)
        {

        }

        public DateTime? GetLastLoginDate(string userId)
        {
            return context
                 .Where(u => u.Title.StartsWith(userId) && u.UserAction == 2)
                 .OrderByDescending(u => u.LoginDate)
                 .Select(u => u.LoginDate)
                 .FirstOrDefault();
        }
    }
}
