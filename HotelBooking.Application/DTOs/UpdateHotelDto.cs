using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Application.DTOs
{
    public class UpdateHotelDto
    {
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Star { get; set; }
    }
}
