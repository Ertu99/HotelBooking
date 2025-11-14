using HotelBooking.Domain.Interfaces;
using HotelBooking.Domain.Repositories;
using HotelBooking.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelBookingDbContext _context;

        public IHotelRepository Hotels { get; }

        public UnitOfWork(HotelBookingDbContext context, IHotelRepository hotelRepository)
        {
            _context = context;
            Hotels = hotelRepository;
        }

       
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
