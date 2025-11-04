using HotelBooking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDto GenerateToken(string email, string role);
    }
}
