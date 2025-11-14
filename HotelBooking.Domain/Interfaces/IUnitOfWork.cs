using HotelBooking.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IHotelRepository Hotels { get; }
        Task<int> SaveChangesAsync();
    }
}
