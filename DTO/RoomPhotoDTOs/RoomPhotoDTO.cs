namespace DTO.RoomPhotoDTOs
{
     public record RoomPhotoDTO 
    {
        public Guid PhotoId { get; set; }
        public string? PhotoPath { get; set; } = null!;
    }
}
