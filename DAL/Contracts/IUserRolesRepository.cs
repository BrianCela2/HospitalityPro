﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
	public interface IUserRolesRepository : IRepository<UserRole, Guid>
	{
		List<UserRole> GetUserRolesById(Guid userId);
		UserRole GetUserRole(Guid userId, int role);
		List<UserRole> GetAllUserRoles();

		//
		public int GetRoleUsersCount(int role);


    }
}
