using Microsoft.EntityFrameworkCore;

namespace CineAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Historic> Historics { get; set; }
        public DbSet<HistoricType> HistoricTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Tokens> Tokens { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}