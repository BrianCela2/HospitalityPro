using DAL.Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    internal class RoomPhotoRepository : BaseRepository<RoomPhoto, Guid>, IRoomPhotoRepository
    {

        public RoomPhotoRepository(HospitalityProContext dbContext) : base(dbContext)
        {
        }
        public IEnumerable<RoomPhoto> roomPhotos(Guid roomid)
        {
            var roomPhotos = context.Where(x=>x.RoomId==roomid).ToList();
            return roomPhotos;
        }
    }
}
