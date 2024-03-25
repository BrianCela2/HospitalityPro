using Helpers.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace DTO.HotelServiceDTOs
{
    public class CreateHotelServiceDTO
    {
        [Required]
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ServiceCategory? Category { get; set; }
        public string? OpenTime { get; set; }
    }
}
