using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    [Table("User_Roles")]
    public partial class UserRole
    {
        [Key]
        [Column("UserID")]
        public Guid UserId { get; set; }
        [Key]
        public int Roles { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("UserRoles")]
        public virtual User User { get; set; } = null!;
    }
}
