using Domain.Contracts;
using Domain.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using Helpers.StaticFunc;
using DTO.RoomDTOs;
namespace HospitalityPro.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IRoomDomain _roomDomain;


        public CartController(IRoomDomain roomDomain)
        {
            _roomDomain = roomDomain;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<RoomItem> cart = HttpContext.Session.GetJson<List<RoomItem>>("Cart") ?? new List<RoomItem>();

            Cart cartVM = new()
            {
                RoomItems = cart,
                TotalPrice = cart.Sum(x => x.Price)
            };

            return Ok(cartVM);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> Add(Guid id )
        {
            RoomDTO room = await _roomDomain.GetRoomByIdAsync(id);

            List<RoomItem> cart = HttpContext.Session.GetJson<List<RoomItem>>("Cart") ?? new List<RoomItem>();

            RoomItem cartItem = cart.Where(c => c.RoomId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new RoomItem(room));
            }

            HttpContext.Session.SetJson("Cart", cart);

            return NoContent();
        }
        [HttpDelete]
        [Route("RoomRemove")]
            public async Task<IActionResult> Remove(Guid id)
            {
                List<RoomItem> cart = HttpContext.Session.GetJson<List<RoomItem>>("Cart");

                cart.RemoveAll(p => p.RoomId == id);

                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetJson("Cart", cart);
                }


                return NoContent();
            }
        [HttpDelete]
        [Route("DeleteCart")]
            public IActionResult Clear()
            {
                HttpContext.Session.Remove("Cart");

                return NoContent();
            }
        
    }
}
