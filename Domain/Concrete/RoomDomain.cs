using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.RoomDTOs;
using DTO.RoomPhotoDTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    internal class RoomDomain : DomainBase, IRoomDomain
    {
        public RoomDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
        }

        private IRoomRepository roomRepository => _unitOfWork.GetRepository<IRoomRepository>();
        private IRoomPhotoRepository roomPhotoRepository => _unitOfWork.GetRepository<IRoomPhotoRepository>();

        public async Task AddRoomAsync(CreateRoomDTO createRoomDTO)
        {
            var room = _mapper.Map<Room>(createRoomDTO);
            roomRepository.Add(room);
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
    }
}
