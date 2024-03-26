using System;
using System.ComponentModel.DataAnnotations;

namespace DTO.HotelServiceDTOs
{
    public class HotelServiceDTO
    {
        public Guid ServiceID { get; set; }
        [Required]
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int? Category { get; set; }
        public string? OpenTime { get; set; }
    }
}
