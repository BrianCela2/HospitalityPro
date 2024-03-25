using AutoMapper;
using DAL.Contracts;
using Domain.Contracts;
using DTO.UserRoles;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    internal class UserRolesDomain : DomainBase, IUserRolesDomain
    {
        private readonly IUserRolesRepository _userRolesRepository;

        public UserRolesDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        {
            _userRolesRepository = unitOfWork.GetUserRolesRepository();
        }

        public async Task AddRoleToUser(UserRoleDTO userRoleDto)
        {
            UserRole userRole = _mapper.Map<UserRole>(userRoleDto);
            _userRolesRepository.Add(userRole);
            await _unitOfWork.SaveAsync();
        }


        public async Task<List<UserRoleDTO>> GetUserRoleById(Guid userId)
        {
            List<UserRole> userRoles = _userRolesRepository.GetUserRolesById(userId);
            if (userRoles == null || userRoles.Count == 0)
            {
                throw new Exception($"Roles with ID {userId} not found");
            }
            var roles = _mapper.Map<List<UserRoleDTO>>(userRoles);
            return roles;
        }

        public async Task RemoveUserRole(Guid userId, int roleId)
        {
            UserRole userRoleToRemove = _userRolesRepository.GetUserRole(userId, roleId);
            if (userRoleToRemove == null)
            {
                throw new Exception($"User {userId} with role {roleId} not found");
            }

            _userRolesRepository.Remove(userRoleToRemove);
            _unitOfWork.Save();
        }
    }
}
