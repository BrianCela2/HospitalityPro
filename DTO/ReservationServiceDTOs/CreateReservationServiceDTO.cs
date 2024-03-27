using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ReservationServiceDTOs
{
    public class CreateReservationServiceDTO
    {
        public Guid ServiceId { get; set; }
        public DateTime DateOfPurchase { get; set; }
    }
}
