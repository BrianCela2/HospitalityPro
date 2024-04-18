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
        IEnumerable<UserDTO> GetAllUsers(int page, int pageSize, string sortField, string sortOrder, string searchString);

		UserDTO GetUserById(Guid id);
        UserDTO GetUserByEmail(string email);
        Task UpdateUserAsync(UserDTO userDTO);

        //
        public int GetActiveUsersCount();

    }
}
