using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ShoppingCart
{
    public class Cart
    {
        public List<RoomItem> RoomItems { get; set; } = null!;
        public decimal TotalPrice { get; set; }
    }
}
