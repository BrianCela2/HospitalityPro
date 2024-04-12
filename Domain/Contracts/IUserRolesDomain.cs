using DTO.RoomPhotoDTOs;
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
		Task<List<UserRoleDTO>> GetUserRoleById(Guid userId);
		Task AddRoleToUser(UserRoleDTO userRoleDto);
		Task RemoveUserRole(Guid userId, int roleId);

		//
		public int GetRoleUsersCount(int role);



    }
}
