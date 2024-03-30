using DTO.UserDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUserDomain
    {
        IList<UserDTO> GetAllUsers();
        UserDTO GetUserById(Guid id);
        UserDTO GetUserByEmail(string email);
        Task UpdateUserAsync(Guid userId,UserDTO userDTO);

        //
        public int GetActiveUsersCount();

    }
}
