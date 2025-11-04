using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminArea()
        {
            return Ok("Welcome, Admin!");
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult UserArea()
        {
            return Ok("Welcome, authenticated user!");
        }
    }
}
