using Microsoft.EntityFrameworkCore;

namespace Travelling_Application.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Photos> ObjectPhotos { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Accomodation> Accomodation { get; set; }

        public DbSet<Entertainment> Entertainment { get; set; }
        public DbSet<AccomodationPhotos> AccomodationPhotos { get; set; }
        public DbSet<RoomPhotos> RoomPhotos { get; set; }

        public DbSet<BookingAttraction> BookingAttractions { get; set; }

        public DbSet<FavoriteAttraction> FavoriteAttractions { get; set; }
        public DbSet<FavoriteCar> FavoriteCars { get; set; }
        public DbSet<FavoriteAccomodation> FavoriteAccomodations { get; set; }
        public DbSet<FavoriteAirTicket> FavoriteAirTickets { get; set; }

        public DbSet<BookingCar> BookingCars { get; set; }

        public DbSet<BookingAirTicket> BookingAirTickets { get; set; }
        public DbSet<BookingAccomodation> BookingAccomodations { get; set; }


        public DbSet<Car> Car { get; set; }

        public DbSet<AirTicket> AirTicket { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
