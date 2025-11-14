using AutoMapper;
using FluentAssertions;
using HotelBooking.Application.DTOs;
using HotelBooking.Application.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests
{
    public class HotelServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IHotelRepository> _repoMock = new();
        private readonly IMapper _mapper;

        public HotelServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateHotelDto, Hotel>();
                cfg.CreateMap<UpdateHotelDto, Hotel>();
                cfg.CreateMap<Hotel, HotelDto>();
            });

            _mapper = config.CreateMapper();
            _uowMock.Setup(u => u.Hotels).Returns(_repoMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnCreatedHotel()
        {
            //arrange
            var service = new HotelService(_uowMock.Object, _mapper);

            var dto = new CreateHotelDto
            {
                Name = "Test",
                City = "Izmir",
                Star = 4
            };

            var entity = _mapper.Map<Hotel>(dto);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Hotel>()))
                .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            //act

            var result = await service.AddAsync(dto);

            //assert
            result.Name.Should().Be("Test");
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Hotel>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnHotelDtos()
        {
            //arrange
            var hotels = new List<Hotel>
            {
                new Hotel { Id = 1, Name = "A", City = "Istanbul" },
                new Hotel { Id = 2, Name = "B", City = "Ankara" }
            };

            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(hotels);

            var service = new HotelService(_uowMock.Object, _mapper);

            //act
            var result = await service.GetAllAsync();

            //assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("A");

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenHotelExists()
        {
            // Arrange
            var hotel = new Hotel { Id = 10, Name = "TestHotel", City = "Bursa" };
            _repoMock.Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(hotel);

            var service = new HotelService(_uowMock.Object, _mapper);

            //act
            var result = await service.GetByIdAsync(10);

            //assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("TestHotel");

        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenHotelDoesNotExist()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((Hotel?)null);

            var service = new HotelService(_uowMock.Object, _mapper);

            // Act
            var result = await service.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateHotel_WhenExists()
        {
            // Arrange
            var existing = new Hotel { Id = 3, Name = "Old", City = "Izmir" };

            var dto = new UpdateHotelDto
            {
                Name = "NewName",
                City = "Antalya",
                Star = 5
            };

            _repoMock.Setup(r=>r.GetByIdAsync(3))
                .ReturnsAsync(existing);

            var service = new HotelService(_uowMock.Object, _mapper);

            //act
            await service.UpdateAsync(3,dto);

            //assert
            _repoMock.Verify(r => r.Update(It.Is<Hotel>(h => h.Name == "NewName")), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldDeleteHotel_WhenExists()
        {
            // Arrange
            var existing = new Hotel { Id = 5, Name = "DeleteMe" };

            _repoMock.Setup(r => r.GetByIdAsync(5))
                     .ReturnsAsync(existing);

            var service = new HotelService(_uowMock.Object, _mapper);

            // Act
            await service.DeleteAsync(5);

            // Assert
            _repoMock.Verify(r => r.Delete(existing), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
