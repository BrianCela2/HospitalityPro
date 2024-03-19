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
    internal class RoomPhotoDomain : DomainBase, IRoomPhotoDomain
    {
        public RoomPhotoDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
        }

        private IRoomPhotoRepository roomPhotoRepository => _unitOfWork.GetRepository<IRoomPhotoRepository>();
        public async Task AddPhotoAsync(CreateRoomPhotoDTO createRoomPhotoDTO)
        {
            var photo = _mapper.Map<RoomPhoto>(createRoomPhotoDTO);

            var file = createRoomPhotoDTO.ImageFile;
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var photoData = stream.ToArray();
                photo.PhotoContent = photoData;
                photo.PhotoPath = fileName;
                
            }
            roomPhotoRepository.Add(photo);
            _unitOfWork.Save();

        }
        public async Task<RoomPhotoDTO> GetPhotoByIdAsync(Guid id)
        {
            var photo = roomPhotoRepository.GetById(id);
            var photoDTO = _mapper.Map<RoomPhotoDTO>(photo);
            if (photo == null)
            {
                throw new Exception($"Room with ID {id} not found");
            }
            return photoDTO;
        }
        public async Task DeletePhotoAsync(Guid id)
        {

            RoomPhoto roomphoto =  roomPhotoRepository.GetById(id);
            if (roomphoto != null)
            {
                roomPhotoRepository.Remove(id);
                _unitOfWork.Save();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
