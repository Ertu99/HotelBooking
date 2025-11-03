using AutoMapper;
using HotelBooking.Application.DTOs;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
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
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HotelDto> AddAsync(CreateHotelDto dto)
        {
            var entity = _mapper.Map<Hotel>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<HotelDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<HotelDto>> GetAllAsync()
        {
            var hotels = await _repository.GetAllAsync();
            return _mapper.Map<List<HotelDto>>(hotels);
        }

        public async Task<HotelDto?> GetByIdAsync(int id)
        {
            var hotel = await _repository.GetByIdAsync(id);
            return hotel == null ? null : _mapper.Map<HotelDto>(hotel);
        }

        public async Task UpdateAsync(int id, UpdateHotelDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if(existing == null)
                throw new Exception("Hotel not found.");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);

        }
    }
}
