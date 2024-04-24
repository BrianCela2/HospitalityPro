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
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    internal class UserRolesDomain : DomainBase, IUserRolesDomain
    {
		private readonly PaginationHelper<UserRoleDetailDTO> _paginationHelper;


		public UserRolesDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
			_paginationHelper = new PaginationHelper<UserRoleDetailDTO>();

		}

		private IUserRepository userRepository => _unitOfWork.GetRepository<IUserRepository>();
		private IUserRolesRepository userRolesRepository => _unitOfWork.GetRepository<IUserRolesRepository>();

		public async Task<UserRole> AddRoleToUser(UserRoleDTO userRoleDto)
		{
			var existingUserRole =  userRolesRepository.GetUserRole(userRoleDto.UserId, (int)userRoleDto.Roles);

			if (existingUserRole != null)
			{
				throw new Exception($"Role already exists");
			}

			UserRole userRole = _mapper.Map<UserRole>(userRoleDto);

			userRolesRepository.Add(userRole);
			_unitOfWork.Save();
			return null; 
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

		public async Task<IEnumerable<UserRoleDetailDTO>> GetUserRoleDetailsAsync(int page, int pageSize, string sortField, string sortOrder, string searchString)
		{
			searchString = searchString?.ToLower();
			IEnumerable<UserRole> userRoles = userRolesRepository.GetAllUserRoles();
			var mappedUserRoles = _mapper.Map<IEnumerable<UserRoleDetailDTO>>(userRoles);
			Func<UserRoleDetailDTO, bool> filterFunc = u => string.IsNullOrEmpty(searchString) || u.FirstName.ToLower().Contains(searchString) || u.LastName.Contains(searchString);
			IEnumerable<UserRoleDetailDTO> paginatedUserRole = _paginationHelper.GetPaginatedData(mappedUserRoles, page, pageSize, sortField, sortOrder, searchString, filterFunc: filterFunc);
			return paginatedUserRole;
		}
	}
}