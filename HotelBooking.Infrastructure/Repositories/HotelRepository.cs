using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Repositories;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {

        private readonly HotelBookingDbContext _context;
        public HotelRepository(HotelBookingDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);

        }

        public void Delete(Hotel hotel)
        {
             _context.Hotels.Remove(hotel);
        }

        public async Task<List<Hotel>> GetAllAsync()
        {
            return await _context.Hotels
                .AsNoTracking()
                .Include(h => h.Rooms)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels
                .AsNoTracking()
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);
            
        }

        public void Update(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
           
        }
    }
}
