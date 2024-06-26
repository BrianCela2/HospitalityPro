﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        User GetById(Guid id);
        User GetByEmail(string email);

        //
        public int GetActiveUsersCount();

    }
}