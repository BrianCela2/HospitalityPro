using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    [Keyless]
    [Table("User_History")]
    public partial class UserHistory
    {
        [Column("ID")]
        public Guid? Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LoginDate { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Title { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Browser { get; set; }
        public int? UserAction { get; set; }
    }
}
