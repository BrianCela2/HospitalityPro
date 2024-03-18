using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models
{
    public partial class HotelService
    {
        public HotelService()
        {
            Reservations = new HashSet<Reservation>();
        }

        [Key]
        [Column("ServiceID")]
        public Guid ServiceId { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string ServiceName { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string? Description { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public int? Category { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? OpenTime { get; set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("Services")]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
