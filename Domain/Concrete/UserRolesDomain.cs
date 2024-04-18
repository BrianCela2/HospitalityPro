using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.ReservationsDTOS;
using DTO.RoomDTOs;
using DTO.UserDTO;
using DTO.UserRoleDTO;
using DTO.UserRoles;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    internal class UserRolesDomain : DomainBase, IUserRolesDomain
    {
        public UserRolesDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
        }

		private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
		private IUserRolesRepository userRolesRepository => _unitOfWork.GetRepository<IUserRolesRepository>();

        public async Task AddRoleToUser(UserRoleDTO userRoleDto)
        {
            UserRole userRole = _mapper.Map<UserRole>(userRoleDto);
            userRolesRepository.Add(userRole);
            _unitOfWork.Save();
        }


        public async Task<List<UserRoleDTO>> GetUserRoleById(Guid userId)
        {
            List<UserRole> userRoles = userRolesRepository.GetUserRolesById(userId);
            if (userRoles == null)
            {
                throw new Exception($"Roles with ID {userId} not found");
            }
            var roles = _mapper.Map<List<UserRoleDTO>>(userRoles);
            return roles;
        }

        public async Task RemoveUserRole(UserRoleDTO userRole)
        {
			var role = _mapper.Map<UserRole>(userRole);
            userRolesRepository.Remove(role);
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync()
        {

			IEnumerable<UserRole> userRole = userRolesRepository.GetAll();
			var userRoles = _mapper.Map<IList<UserRoleDTO>>(userRole);
			return userRoles;
		}

        //
        public int GetRoleUsersCount(int role)
        {
            return userRolesRepository.GetRoleUsersCount(role);
        }

		public async Task<IEnumerable<UserRoleDetailDTO>> GetUserRoleDetailsAsync()
		{
			IEnumerable<UserRole> userRoles = userRolesRepository.GetAll();
			var mappedUserRoles = _mapper.Map<IEnumerable<UserRoleDetailDTO>>(userRoles);
			foreach (var userRole in mappedUserRoles)
            {
				var user = userRepository.GetById(userRole.UserId);
                userRole.FirstName = user.FirstName;
                userRole.LastName = user.LastName;
			}
            return mappedUserRoles;
		}
	}
}