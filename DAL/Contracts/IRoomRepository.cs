using Entities.Models;
using Entities.SearchParametersList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IRoomRepository : IRepository<Room, Guid>
    {
        IEnumerable<Room> GetAllRoomsPhoto();
        List<List<Room>> GetRoomsAvailable(List<SearchParameters> searchParameters) ;
    }
}
