using DTO.RoomPhotoDTOs;
using Helpers.Enumerations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DTO.RoomDTOs
{
    public class RoomDTO
    {
        public Guid RoomId { get; set; }
        [Required]
        public int RoomNumber { get; set; }
        public int? Capacity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public RoomStatus? RoomStatus { get; set; }
        public RoomCategory? Category { get; set; }
        [NotMapped]
        [JsonIgnore]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        public List<IFormFile>? Photos { get; set; }
        public virtual ICollection<RoomPhotoDTO>? RoomPhotos { get; set; }
    }
   
}


