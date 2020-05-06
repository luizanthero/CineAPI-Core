using Microsoft.EntityFrameworkCore;

namespace CineAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Screen> Screens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
