using DTO.RoomDTOs;
using Helpers.Enumerations;

namespace Domain.ShoppingCart
{
    public class RoomItem
    {
        public Guid RoomId { get; set; }
        public RoomCategory? Category { get; set; }
        public decimal Price { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; } 
        public RoomItem()
        {
        }
        public RoomItem(RoomDTO roomDTO)
        {
            RoomId = roomDTO.RoomId;
            Category = roomDTO.Category;
            Price = roomDTO.Price;
            //reservationRoom = roomDTO.Reservation;
        }
    }
}
