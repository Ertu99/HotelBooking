using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginDto dto)
        {
            if(dto.Email == "admin@test.com" && dto.Password =="1234")
            {
                var token = _authService.GenerateToken(dto.Email, "Admin");
                return Ok(token);
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
