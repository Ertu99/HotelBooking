using AutoMapper;
using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public HotelService(IUnitOfWork uow,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<HotelDto> AddAsync(CreateHotelDto dto)
        {
            var entity = _mapper.Map<Hotel>(dto);
            await _uow.Hotels.AddAsync(entity);
            await _uow.SaveChangesAsync();
            return _mapper.Map<HotelDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _uow.Hotels.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Hotel not Found");

            _uow.Hotels.Delete(existing);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<HotelDto>> GetAllAsync()
        {
            var hotels = await _uow.Hotels.GetAllAsync();

            return _mapper.Map<List<HotelDto>>(hotels);
        }

        public async Task<HotelDto?> GetByIdAsync(int id)
        {
            var hotel = await _uow.Hotels.GetByIdAsync(id);
            return hotel == null ? null : _mapper.Map<HotelDto>(hotel);
        }

        public async Task UpdateAsync(int id, UpdateHotelDto dto)
        {
            var existing = await _uow.Hotels.GetByIdAsync(id);
            if(existing == null)
                throw new Exception("Hotel not found.");

            _mapper.Map(dto,existing);
            _uow.Hotels.Update(existing);
            await _uow.SaveChangesAsync(); 

        }
    }
}
