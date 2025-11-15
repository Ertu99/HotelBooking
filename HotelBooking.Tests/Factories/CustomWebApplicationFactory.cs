using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using HotelBooking.Api;
using System;
using System.Linq;

namespace HotelBooking.Tests.Factories
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove any registration that references HotelBookingDbContext or its options.
                var descriptorsToRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<HotelBookingDbContext>) ||
                    d.ServiceType == typeof(HotelBookingDbContext) ||
                    (d.ImplementationType != null && d.ImplementationType.Name == nameof(HotelBookingDbContext)) ||
                    (d.ServiceType?.FullName?.Contains(nameof(HotelBookingDbContext)) ?? false)
                ).ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                // Add a fresh InMemory DB for integration tests.
                services.AddDbContext<HotelBookingDbContext>(options =>
                {
                    // Use a fixed name or Guid.NewGuid().ToString() to isolate per test run.
                    options.UseInMemoryDatabase("IntegrationTestDB");
                });

                // Build provider and initialize DB
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<HotelBookingDbContext>();

                // Ensure clean database (optional: EnsureDeleted to start fresh)
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            });
        }
    }
}
