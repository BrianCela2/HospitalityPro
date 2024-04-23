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
        private readonly INotificationDomain _notificationDomain;
		private readonly PaginationHelper<Room> _paginationHelper;
		public RoomDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> notificationHubContext, INotificationDomain notificationDomain) : base(unitOfWork, mapper, httpContextAccessor)
        {
            _notificationHubContext = notificationHubContext;
            _notificationDomain = notificationDomain;
			_paginationHelper = new PaginationHelper<Room>();
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
        }

        public async Task<RoomDTO> GetRoomByIdAsync(Guid id)
        {
            var room = roomRepository.GetById(id);
            if (room == null){
                throw new Exception($"Room with ID {id} not found");
            }
            return  _mapper.Map<RoomDTO>(room);
        }
        public async Task<PaginatedRoomDTO> GetAllRoomAsync(int page, int pageSize, string sortField, string sortOrder)
        {
				IEnumerable<Room> rooms = roomRepository.GetAll();
				IEnumerable<Room> paginatedRooms = _paginationHelper.GetPaginatedData(rooms, page, pageSize, sortField, sortOrder);
			    var allRooms = _mapper.Map<IEnumerable<RoomDTO>>(paginatedRooms);
			    var totalRoomsCount = rooms.Count();
			    var totalPages = (int)Math.Ceiling((double)totalRoomsCount / pageSize);
                return new PaginatedRoomDTO
                {
                    Rooms = allRooms,
                    TotalPages = totalPages
                };
		}

        public async Task<PaginatedRoomDTO> GetRoomPhotos(int page, int pageSize, string sortField, string sortOrder)
        {
			IEnumerable<Room> rooms = roomRepository.GetAllRoomsPhoto();
			IEnumerable<Room> paginatedRooms = _paginationHelper.GetPaginatedData(rooms, page, pageSize, sortField, sortOrder);
			var allRooms = _mapper.Map<IEnumerable<RoomDTO>>(paginatedRooms);
			var totalRoomsCount = rooms.Count();
			var totalPages = (int)Math.Ceiling((double)totalRoomsCount / pageSize);
			return new PaginatedRoomDTO
			{
				Rooms = allRooms,
				TotalPages = totalPages
			};
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
            }

            IEnumerable<ReservationRoom> roomReservations = roomReservationRepository.GetReservationByRoom(room.RoomId);
            if (roomReservations.Any())
            {
                roomReservationRepository.RemoveRange(roomReservations);
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
            List<Guid> reservedRoomIds = new List<Guid>();

            foreach (var criteria in searchParameters)
            {
                List<Room?> availableRooms = roomRepository.GetAllRoomsPhoto()
                    .Where(room => room.Capacity >= criteria.Capacity && !reservedRoomIds.Contains(room.RoomId))
                    .GroupBy(room => room.Category)
                    .Select(x => x.FirstOrDefault(room =>
                        roomReservationRepository.GetRoomIncludeReservation(room.RoomId)
                            .All(reservation =>
                                criteria.CheckOutDate <= reservation.CheckInDate ||
                                criteria.CheckInDate >= reservation.CheckOutDate ||
                                reservation.Reservation.ReservationStatus == 2)))
                    .Where(room => room != null)
                    .ToList();

                reservedRoomIds.AddRange(availableRooms.Select(room => room.RoomId));

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
