﻿using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.UserDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.JWT;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Domain.Concrete
{
    internal class UserDomain : DomainBase, IUserDomain
    {
        public UserDomain(IUnitOfWork unitOfWork,IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
		}

        private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
        public IList<UserDTO> GetAllUsers()
        {
            IEnumerable<User> user = userRepository.GetAll();
            var test = _mapper.Map<IList<UserDTO>>(user);
            return test;
        }

        public UserDTO GetUserById(Guid id)
        {
            User user = userRepository.GetById(id);
			return _mapper.Map<UserDTO>(user);

		}

		public UserDTO GetUserByEmail(string email)
		{
			User user = userRepository.GetByEmail(email);
			return _mapper.Map<UserDTO>(user);
		}
	}
}