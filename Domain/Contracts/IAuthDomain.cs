using DTO.UserDTO;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
	public interface IAuthDomain
	{
		Task<User> Register(RegisterDTO request);
		Task<string> Login(LoginDTO request);
	}
}
