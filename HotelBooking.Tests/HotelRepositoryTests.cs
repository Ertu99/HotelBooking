using FluentAssertions;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests
{
    public class HotelRepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddHotelToDatabase()
        {

            //Arrange
            var db = TestDbContextFactory.CreateDbContext("HotelDb_Add");
            var repo = new HotelRepository(db);

            var hotel = new Hotel
            {
                Name = "Test Hotel",
                City = "Istanbul",
                Star = 5
            };

            //act
            await repo.AddAsync(hotel);
            await db.SaveChangesAsync();

            //assert
            db.Hotels.Count().Should().Be(1);
            db.Hotels.First().Name.Should().Be("Test Hotel");

        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllHotels()
        {
            var db = TestDbContextFactory.CreateDbContext("HotelDb_GetAll");
            var repo = new HotelRepository(db);

            db.Hotels.Add(new Hotel { Name = "A", City = "Ankara" });
            db.Hotels.Add(new Hotel { Name = "B", City = "Bursa" });
            await db.SaveChangesAsync();

            //act
            var result = await repo.GetAllAsync();

            //assert
            result.Should().HaveCount(2);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnHotel_WhenHotelExists()
        {
            //arrange
            var db = TestDbContextFactory.CreateDbContext("HotelDb_GetById_1");
            var repo = new HotelRepository(db);

            var hotel = new Hotel
            {
                Id = 10,
                Name = "Test Hotel",
                City = "Izmir",
                Star = 4
            };

            db.Hotels.Add(hotel);
            await db.SaveChangesAsync();

            // Act
            var result = await repo.GetByIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Hotel");
            result.City.Should().Be("Izmir");
            result.Star.Should().Be(4);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            var db = TestDbContextFactory.CreateDbContext("HotelDb_GetById_2");
            var repo = new HotelRepository(db);

            // Act
            var result = await repo.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveHotelFromDatabase()
        {
            // Arrange
            var db = TestDbContextFactory.CreateDbContext("HotelDb_Delete");
            var repo = new HotelRepository(db);

            var hotel = new Hotel
            {
                Id = 5,
                Name = "DeleteTest",
                City = "Adana"
            };

            db.Hotels.Add(hotel);
            await db.SaveChangesAsync();

            // Act
            repo.Delete(hotel);
            await db.SaveChangesAsync();

            // Assert
            db.Hotels.Count().Should().Be(0);
        }
    }
}
