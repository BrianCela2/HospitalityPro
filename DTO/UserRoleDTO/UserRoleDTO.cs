﻿using Helpers.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserRoles
{
	public class UserRoleDTO
	{
		public Guid UserId { get; set; }
		public Roles Roles { get; set; }
	}
}
