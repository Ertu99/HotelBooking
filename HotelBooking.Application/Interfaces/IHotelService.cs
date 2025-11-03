using HotelBooking.Application.DTOs;
using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IHotelService
    {
        Task<List<HotelDto>> GetAllAsync();
        Task<HotelDto?> GetByIdAsync(int id);
        Task<HotelDto> AddAsync(CreateHotelDto dto);
        Task UpdateAsync(int id, UpdateHotelDto dto);
        Task DeleteAsync(int id);
    }
}
