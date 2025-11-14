using HotelBooking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests
{
    public static class TestDbContextFactory
    {
        public static HotelBookingDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<HotelBookingDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new HotelBookingDbContext(options);
        }
    }
}
