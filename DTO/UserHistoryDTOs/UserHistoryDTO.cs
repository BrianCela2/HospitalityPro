using Helpers.Enumerations;

namespace DTO.UserHistoryDTOs
{
    public record UserHistoryDTO
    {
        public DateTime? LoginDate { get; set; }
        public string? Title { get; set; }
        public string? Browser { get; set; }
        public UserAction? UserAction { get; set; }
    }
}
