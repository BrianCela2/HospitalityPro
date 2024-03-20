using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
	internal class UserRolesRepository : BaseRepository<UserRole, Guid>, IUserRolesRepository
	{
		public UserRolesRepository(HospitalityProContext dbContext) : base(dbContext)
		{
		}

		public UserRole GetUserRole(Guid userId, int role)
		{ 
			UserRole userRole = context.Where(ur => ur.UserId == userId && ur.Roles == role).FirstOrDefault();
			return userRole;
		}

		public List<UserRole> GetUserRolesById(Guid userId)
		{
			var userRoles = context.Where(a => a.UserId == userId).ToList();
			return userRoles;
		}
	}
}
