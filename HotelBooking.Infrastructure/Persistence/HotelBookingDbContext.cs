using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Infrastructure.Persistence
{
    public class HotelBookingDbContext : DbContext
    {

        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels => Set<Hotel>();
        public DbSet<Room> Rooms => Set<Room>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>(b =>
            {
                b.ToTable("Hotels");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.City).HasMaxLength(100).IsRequired();
                b.Property(x => x.Star).HasDefaultValue(3);

                //b.Property(x => x.RowVersion)
                //.IsRowVersion();
                
            });

            modelBuilder.Entity<Room>(b =>
            {
                b.ToTable("Rooms");
                b.HasKey(x => x.Id);
                b.Property(x => x.Number).HasMaxLength(20).IsRequired();
                b.Property(x => x.PricePerNight).HasColumnType("decimal(18,2)");
                b.HasOne(x => x.Hotel)
                 .WithMany(h => h.Rooms)
                 .HasForeignKey(x => x.HotelId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
