using DTO.RoomPhotoDTOs;
using DTO.UserRoleDTO;
using DTO.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
	public interface IUserRolesDomain
	{
		Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync();
		Task<IEnumerable<UserRoleDetailDTO>> GetUserRoleDetailsAsync();
		Task<List<UserRoleDTO>> GetUserRoleById(Guid userId);
		Task AddRoleToUser(UserRoleDTO userRoleDto);
		Task RemoveUserRole(UserRoleDTO userRole);

		//
		public int GetRoleUsersCount(int role);



    }
}
