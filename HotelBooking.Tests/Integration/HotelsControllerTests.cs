using FluentAssertions;
using HotelBooking.Application.DTOs;
using HotelBooking.Tests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Tests.Integration
{
    public class HotelsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public HotelsControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturn200()
        {
            // act
            var response = await _client.GetAsync("/api/Hotels");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedHotel()
        {
            var dto = new CreateHotelDto
            {
                Name = "TestHotel",
                City = "Istanbul",
                Star = 5
            };

            var response = await _client.PostAsJsonAsync("/api/Hotels", dto);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result = await response.Content.ReadFromJsonAsync<HotelDto>();

            result.Should().NotBeNull();
            result!.Name.Should().Be("TestHotel");
        }
        [Fact]
        public async Task GetById_ShouldReturnHotel_WhenExists()
        {
            
            var createDto = new CreateHotelDto
            {
                Name = "GetByIdHotel",
                City = "Izmir",
                Star = 4
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Hotels", createDto);
            var created = await createResponse.Content.ReadFromJsonAsync<HotelDto>();

            // 2) Get by id
            var response = await _client.GetAsync($"/api/Hotels/{created!.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var hotel = await response.Content.ReadFromJsonAsync<HotelDto>();
            hotel!.Name.Should().Be("GetByIdHotel");
        }

        [Fact]
        public async Task Update_ShouldModifyHotel()
        {
            // 1) Create hotel
            var createDto = new CreateHotelDto
            {
                Name = "OldName",
                City = "Ankara",
                Star = 3
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Hotels", createDto);
            var created = await createResponse.Content.ReadFromJsonAsync<HotelDto>();

            // 2) Update
            var updateDto = new UpdateHotelDto
            {
                Name = "NewName",
                City = "Bursa",
                Star = 5
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/Hotels/{created!.Id}", updateDto);

            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // 3) Tekrar getir ve doğrula
            var getResponse = await _client.GetAsync($"/api/Hotels/{created.Id}");
            var hotel = await getResponse.Content.ReadFromJsonAsync<HotelDto>();

            hotel!.Name.Should().Be("NewName");
        }
        [Fact]
        public async Task Delete_ShouldRemoveHotel()
        {
            // 1) Create
            var dto = new CreateHotelDto
            {
                Name = "ToBeDeleted",
                City = "Kayseri",
                Star = 2
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Hotels", dto);
            var created = await createResponse.Content.ReadFromJsonAsync<HotelDto>();

            // 2) Delete
            var deleteResponse = await _client.DeleteAsync($"/api/Hotels/{created!.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // 3) Tekrar çek -> NotFound
            var getResponse = await _client.GetAsync($"/api/Hotels/{created.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}

