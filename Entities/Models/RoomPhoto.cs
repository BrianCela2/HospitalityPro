using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    [Table("Room_Photos")]
    public partial class RoomPhoto
    {
        [Key]
        [Column("PhotoID")]
        public Guid PhotoId { get; set; }
        [Unicode(false)]
        public string PhotoPath { get; set; } = null!;
        [Column("RoomID")]
        public Guid? RoomId { get; set; }

        [ForeignKey("RoomId")]
        [InverseProperty("RoomPhotos")]
        public virtual Room? Room { get; set; }
    }
}
