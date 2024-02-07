using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SkinTone> SkinTones { get; set; } 
        public DbSet<Color> Colors { get; set; }
        public DbSet<SkinToneColorCompatibility> SkinToneColorCompatibilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Role>()
            .Property(e => e.UserRole)
            .HasConversion(
                v => v.ToString(),
                v => (UserRole)Enum.Parse(typeof(UserRole), v)
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, UserRole = UserRole.Admin },
                new Role { Id = 2, UserRole = UserRole.Customer }
            );

            modelBuilder.Entity<SkinToneColorCompatibility>().HasNoKey();
        }
    }
}
