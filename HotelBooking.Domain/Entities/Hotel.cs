using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Star { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
