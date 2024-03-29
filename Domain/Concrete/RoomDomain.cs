using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO.RoomDTOs;
using Entities.Models;
using DTO.SearchParametersList;
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
            if (createRoomDTO.Photos != null)
            {
                var roomPhotoList = new List<RoomPhoto>();
                foreach (var file in createRoomDTO.Photos)
                {
                    var roomPhoto = new RoomPhoto();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var photoData = stream.ToArray();
                        roomPhoto.PhotoContent = photoData;
                        roomPhoto.PhotoPath = file.FileName;
                        roomPhoto.RoomId = room.RoomId;
                        roomPhotoList.Add(roomPhoto);   
                    }
                }
                room.RoomPhotos = roomPhotoList;
               
            }
            roomRepository.Add(room);
            _unitOfWork.Save();
        }
        public async Task<RoomDTO> GetRoomByIdAsync(Guid id)
        {
            var room = roomRepository.GetById(id);
            if (room == null){
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
        public List<List<RoomDTO>> GetRoomsAvailable(List<SearchParameters> searchParameters)
        {
            List<List<RoomDTO>> availableRoomsList = new List<List<RoomDTO>>();
            foreach (var criteria in searchParameters)
            {
                List<Room> availableRooms = roomRepository.GetAllRoomsPhoto()
                    .Where(room => room.Capacity >= criteria.Capacity &&
                                   !room.ReservationRooms.Any(reservation =>
                                        !(criteria.CheckOutDate <= reservation.CheckInDate ||
                                          criteria.CheckInDate >= reservation.CheckOutDate)))
                    .ToList();
                var availableRoomsDTO = _mapper.Map<List<RoomDTO>>(availableRooms);
                availableRoomsList.Add(availableRoomsDTO);
            }
            return availableRoomsList;
        }
        public async Task UpdateRoomStatus(int status ,RoomDTO roomDTO)
        {
            var room = _mapper.Map<Room>(roomDTO);
            room.RoomStatus = status;
            if (status == room.RoomStatus){
                throw new Exception("Room is in that status already");
            }
            else if (status >= 1 && status <= 4){
                roomRepository.Update(room);
                _unitOfWork.Save();
            }
            else{
                throw new Exception();
            }
        }
    }
}
