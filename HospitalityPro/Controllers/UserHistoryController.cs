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
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, string sortField = "LoginDate", string sortOrder = "dsc")
        {
            var historyList = await _userHistoryDomain.GetHistory(page, pageSize, sortField, sortOrder);
            return Ok(historyList);
        }
    }
}
