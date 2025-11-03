using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Number { get; set; } = null!;
        public decimal PricePerNight { get; set; }

        public Hotel? Hotel { get; set; }
    }
}
