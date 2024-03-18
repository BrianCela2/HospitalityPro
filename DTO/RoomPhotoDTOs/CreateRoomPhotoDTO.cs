using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO.RoomPhotoDTOs
{
    public record CreateRoomPhotoDTO
    {
        public Guid? RoomId { get; set; }
        [JsonIgnore]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
