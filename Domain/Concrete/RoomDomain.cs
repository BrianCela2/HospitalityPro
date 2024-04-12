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
using DTO.NotificationDTOs;

namespace Domain.Concrete
{
    internal class RoomDomain : DomainBase, IRoomDomain
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly INotificationDomain _notificationDomain;
        public RoomDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> notificationHubContext, INotificationDomain notificationDomain) : base(unitOfWork, mapper, httpContextAccessor)
        {
            _notificationHubContext = notificationHubContext;
            _notificationDomain = notificationDomain;
        }
        private INotificationRepository notificationRepository => _unitOfWork.GetRepository<INotificationRepository>();
        private IRoomRepository roomRepository => _unitOfWork.GetRepository<IRoomRepository>();
        private IReservationRoomRepository roomReservationRepository => _unitOfWork.GetRepository<IReservationRoomRepository>();
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

            var notification = new CreateNotificationDTO { };
            notification.ReceiverId = new Guid("0bd99a5b-83c2-4ca3-9b20-c1a5ee42f099");
            notification.SenderId = new Guid("ccb7d4e4-5fa5-4616-a1cd-ecf7c9a730a2");
            notification.MessageContent = " U shtua nje dhome e Re ";
            notification.SendDateTime = DateTime.Now;
            await _notificationHubContext.Clients.All.SendAsync("ReceiveNotificationAllUser", notification.MessageContent);
            await _notificationDomain.AddNotificationsAllUserAsync(notification);
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
        public RoomDTO GetRoomWithPhoto(Guid id)
        {
            var room = roomRepository.GetRoomWithPhotos(id);
            if (room == null)
            {
                throw new Exception($"Room with ID {id} not found");
            }
            return _mapper.Map<RoomDTO>(room);
        }
        public async Task DeleteRoom(RoomDTO roomDTO)
        {
            Room room = _mapper.Map<Room>(roomDTO);

            if (!roomRepository.IsDetached(room))
            {
                roomRepository.Detach(room);
            }

            IEnumerable<RoomPhoto> roomPhotos = roomPhotoRepository.roomPhotos(room.RoomId);
            if (roomPhotos.Any())
            {
                roomPhotoRepository.RemoveRange(roomPhotos);
                _unitOfWork.Save();
            }

            IEnumerable<ReservationRoom> roomReservations = roomReservationRepository.GetReservationByRoom(room.RoomId);
            if (roomReservations.Any())
            {
                roomReservationRepository.RemoveRange(roomReservations);
                _unitOfWork.Save();
            }

            if (room != null)
            {
                roomRepository.Remove(room);
                _unitOfWork.Save(); 
            }
            else
            {
                throw new ArgumentNullException(nameof(room));
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
                List<Room> availableRooms = new List<Room>();

                var allRooms = roomRepository.GetAllRoomsPhoto();

                foreach (var room in allRooms)
                {
                    if (room.Capacity >= criteria.Capacity && room.RoomStatus == 1)
                    {
                        bool isRoomAvailable = true;

                        foreach (var reservation in roomReservationRepository.GetReservationByRoom(room.RoomId))
                        {
                            if (!(criteria.CheckOutDate <= reservation.CheckInDate || criteria.CheckInDate >= reservation.CheckOutDate))
                            {
                                isRoomAvailable = false;
                                break;
                            }
                        }

                        if (isRoomAvailable)
                        {
                            availableRooms.Add(room);
                        }
                    }
                }

                var availableRoomsDTO = _mapper.Map<List<RoomDTO>>(availableRooms);
                availableRoomsList.Add(availableRoomsDTO);
            }

            return availableRoomsList;
        }

            public async Task UpdateRoomStatus(int status ,RoomDTO roomDTO)
        {
            var room = _mapper.Map<Room>(roomDTO);
            if (status == room.RoomStatus){
                throw new Exception("Room is in that status already");
            }
            else if (status >= 1 && status <= 4){
                room.RoomStatus = status;
                roomRepository.Update(room);
                _unitOfWork.Save();
            }
            else{
                throw new Exception();
            }
        }

        public int GetAvailableRoomsCount()
        {
            return roomRepository.GetAvailableRoomsCount();
        }


    }
}
