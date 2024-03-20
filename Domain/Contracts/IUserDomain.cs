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
		Task<User> Register(RegisterDTO request);
		Task<string> Login(LoginDTO request);
	}
}
