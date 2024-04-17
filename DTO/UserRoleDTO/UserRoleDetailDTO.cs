using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserRoleDTO
{
	public class UserRoleDetailDTO
	{
		public Guid UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Roles Roles { get; set; }
	}
}
