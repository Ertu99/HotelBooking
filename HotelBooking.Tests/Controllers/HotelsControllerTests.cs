using FluentAssertions;
using HotelBooking.Api.Controllers;
using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests.Controllers
{
    public class HotelsControllerTests
    {
        private readonly Mock<IHotelService> _serviceMock = new();
        private readonly Mock<ILogger<HotelsController>> _loggerMock = new();
        private readonly HotelsController _controller;

        public HotelsControllerTests()
        {
            _controller = new HotelsController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithHotelList()
        {
            // Arrange
            var hotels = new List<HotelDto>
    {
        new HotelDto { Id = 1, Name = "A" },
        new HotelDto { Id = 2, Name = "B" }
    };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(hotels);

            // Act
            var result = await _controller.GetALl();

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(200);

            var value = ok.Value as List<HotelDto>;
            value.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenHotelExists()
        {
            // Arrange
            var dto = new HotelDto { Id = 5, Name = "TestHotel" };
            _serviceMock.Setup(s => s.GetByIdAsync(5))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.GetById(5);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenHotelDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(10))
                .ReturnsAsync((HotelDto?)null);

            // Act
            var result = await _controller.GetById(10);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var dto = new CreateHotelDto { Name = "Test", City = "Izmir", Star = 4 };
            var returnDto = new HotelDto { Id = 1, Name = "Test" };

            _serviceMock.Setup(s => s.AddAsync(dto))
                .ReturnsAsync(returnDto);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.StatusCode.Should().Be(201);
            created.RouteValues!["id"].Should().Be(1);
        }
        [Fact]
        public async Task Update_ShouldReturnNoContent()
        {
            // Arrange
            var dto = new UpdateHotelDto { Name = "Updated" };

            _serviceMock.Setup(s => s.UpdateAsync(1, dto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}