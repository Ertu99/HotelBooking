using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(IHotelService hotelService, ILogger<HotelsController> logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult>GetALl()
        {
            _logger.LogInformation("GetAll called at {time}", DateTime.UtcNow);
            var hotels = await _hotelService.GetAllAsync();
            _logger.LogInformation("Returning {count} hotels",hotels.Count);
            return Ok(hotels);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHotelDto dto)
        {
            var hotel = await _hotelService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching hotel with id={id}", id);
            var hotel = await _hotelService.GetByIdAsync(id);
            
            if (hotel == null)
            {
                _logger.LogWarning("Hotel with id={id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Hotel with id={id} found",id);
            return Ok(hotel);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHotelDto dto)
        {
            await _hotelService.UpdateAsync(id, dto);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _hotelService.DeleteAsync(id);
            return NoContent();
        }
    }
}
