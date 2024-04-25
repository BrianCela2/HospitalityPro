using Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalityPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryDomain _userHistoryDomain;
       public UserHistoryController(IUserHistoryDomain userHistoryDomain)
        {
            _userHistoryDomain = userHistoryDomain;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var historyList = _userHistoryDomain.GetHistory();
            return Ok(historyList);
        }
    }
}
