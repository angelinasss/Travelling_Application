using Microsoft.EntityFrameworkCore;

namespace Travelling_Application.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Accomodation> Accomodation { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //string adminRoleName = "admin";
            //string travellerRoleName = "traveller";
            //string ownerRoleName = "owner";

            //string adminUserName = "Admin_Angelina_2808";
            //string adminPassword = "28082005";

            //// добавляем роли
            //Role adminRole = new Role { Id = 1, Name = adminRoleName };
            //Role travellerRole = new Role { Id = 2, Name = travellerRoleName };
            //Role ownerRole = new Role { Id = 3, Name = ownerRoleName };
            //User adminUser = new User { Id = 1, UserName = adminUserName, Password = adminPassword, RoleId = adminRole.Id, 
            //    Birthday = new DateTime(), Nationality = "", Name = "", Email = "", Sex = "", PhoneNumber = "", Role = adminRole.Name
            //};

            //modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, travellerRole, ownerRole });
            //modelBuilder.Entity<User>().HasData(new User[] { adminUser });
           // base.OnModelCreating(modelBuilder);
        }
    }
}
