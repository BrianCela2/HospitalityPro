using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO.RoomDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Domain.Concrete
{
    internal class RoomDomain : DomainBase, IRoomDomain
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public RoomDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> notificationHubContext) : base(unitOfWork, mapper, httpContextAccessor)
        {
            _notificationHubContext = notificationHubContext;
        }

        private IRoomRepository roomRepository => _unitOfWork.GetRepository<IRoomRepository>();
        private IRoomPhotoRepository roomPhotoRepository => _unitOfWork.GetRepository<IRoomPhotoRepository>();

        public async Task AddRoomAsync(CreateRoomDTO createRoomDTO)
        {
            var room = _mapper.Map<Room>(createRoomDTO);
            roomRepository.Add(room);
            //room.ReservationRooms.Add(new ReservationRoom
            //{
            //    ReservationId=r
            //    RoomId = roomId,
            //    CheckInDate = checkInDate,
            //    CheckOutDate = checkOutDate
            //});
            _unitOfWork.Save();
            if (createRoomDTO.Photos != null)
            {
                foreach (var file in createRoomDTO.Photos)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var roomPhoto = new RoomPhoto
                    {
                    };
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var photoData = stream.ToArray();
                        roomPhoto.PhotoContent = photoData;
                        roomPhoto.PhotoPath = fileName;
                        roomPhoto.RoomId = room.RoomId;
                    }
                    roomPhotoRepository.Add(roomPhoto);
                }
               
                _unitOfWork.Save();
            }

        }
        public async Task<RoomDTO> GetRoomByIdAsync(Guid id)
        {
            var room = roomRepository.GetById(id);

            if (room == null)
            {
                throw new Exception($"Room with ID {id} not found");
            }
            return  _mapper.Map<RoomDTO>(room);
        }

        public async Task<IEnumerable<RoomDTO>> GetAllRoomAsync()
        {
            IEnumerable<Room> rooms = roomRepository.GetAll();
            var roomsDTO = _mapper.Map<IEnumerable<RoomDTO>>(rooms);
            return roomsDTO;
        }
        public IEnumerable<RoomDTO> GetRoomPhotos()
        {
            var rooms = roomRepository.GetAllRoomsPhoto();
            var roomsDTO = _mapper.Map<IEnumerable<RoomDTO>>(rooms);
            return roomsDTO;
        }
        public async Task DeleteRoom(RoomDTO roomDTO)
        {

            Room room = _mapper.Map<Room>(roomDTO);
            IEnumerable<RoomPhoto> roomPhotos = roomPhotoRepository.roomPhotos(room.RoomId);
            if (roomPhotos.Any())
            {
                roomPhotoRepository.RemoveRange(roomPhotos);
            }
            if (room != null)
            {
                roomRepository.Remove(room);
                _unitOfWork.Save();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public async Task UpdateRoom(UpdateRoomDTO updateroomDTO)
        {
            var room = _mapper.Map<Room>(updateroomDTO);
            roomRepository.Update(room);
            _unitOfWork.Save();

        }
    }
}
