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
		Task<IEnumerable<UserRoleDetailDTO>> GetUserRoleDetailsAsync(int page, int pageSize, string sortField, string sortOrder, string searchString);
		Task<List<UserRoleDTO>> GetUserRoleById(Guid userId);
		Task AddRoleToUser(UserRoleDTO userRoleDto);
		Task RemoveUserRole(UserRoleDTO userRole);

		//
		public int GetRoleUsersCount(int role);



    }
}
