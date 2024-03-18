using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO.RoomPhotoDTOs
{
    public record UpdateRoomPhotoDTO
    {
        public Guid PhotoId { get; set; }
        public Guid? RoomId { get; set; }
        [NotMapped]
        [JsonIgnore]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
