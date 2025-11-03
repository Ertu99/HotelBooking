using AutoMapper;
using HotelBooking.Application.DTOs;
using HotelBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity => DTO
            CreateMap<Hotel, HotelDto>();
            CreateMap<Room, RoomDto>();

            //DTO => Entity
            CreateMap<CreateHotelDto, Hotel>();
            CreateMap<UpdateHotelDto, Hotel>();
        }
    }
}
