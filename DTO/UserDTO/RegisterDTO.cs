using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserDTO
{
	public class RegisterDTO
	{
		public string FirstName { get; set; } = null!;
		[StringLength(50)]
		[Unicode(false)]
		public string LastName { get; set; } = null!;
		[StringLength(100)]
		[Unicode(false)]
		public string Email { get; set; } = null!;
		[StringLength(20)]
		[Unicode(false)]
		public string PhoneNumber { get; set; } = null!;
		public string Password { get; set; } = null!;
		[Column(TypeName = "date")]
		public DateTime? Birthday { get; set; }
		[StringLength(30)]
		[Unicode(false)]
		public string Country { get; set; } = null!;
		[StringLength(30)]
		[Unicode(false)]
		public string City { get; set; } = null!;
	}
}
