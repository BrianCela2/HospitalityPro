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

		public List<UserRole> GetUserRolesById(Guid userId)
		{
			var userRoles = context.Where(a => a.UserId == userId).ToList();
			return userRoles;
		}
	}
}
