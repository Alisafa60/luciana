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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Fabric> Fabrics { get; set; }
        public DbSet<SkinToneColorCompatibility> SkinToneColorCompatibilities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<TexturePattern> TexturePatterns { get; set; }
        public DbSet<ProductTexturePattern> ProductTexturePatterns { get; set; }
        public DbSet<ProductColor> ProductColors{ get; set; }
        public DbSet<ProductFabric> ProductFabrics{ get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ParentCategory> ParentCategories { get; set; }
        public DbSet<ParentColor> ParentColors { get; set; }
        public DbSet<ParentFabric> ParentFabrics { get; set; }
        public DbSet<Size> Sizes { get; set; }
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

            modelBuilder.Entity<ProductTexturePattern>()
                .HasKey(ptp => new { ptp.ProductId, ptp.TexturePatternId});

             modelBuilder.Entity<ProductTexturePattern>()
                .HasOne(ptp => ptp.Product)
                .WithMany(p => p.ProductTexturePatterns)
                .HasForeignKey(ptp => ptp.ProductId);

            modelBuilder.Entity<ProductTexturePattern>()
                .HasOne(ptp => ptp.TexturePattern)
                .WithMany()
                .HasForeignKey(ptp => ptp.TexturePatternId);
                
            modelBuilder.Entity<ProductColor>()
                .HasKey(pc => new { pc.ProductId, pc.ColorId});   

            modelBuilder.Entity<ProductColor>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColors)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductColor>()
                .HasOne(pc => pc.Color)
                .WithMany()
                .HasForeignKey(pc => pc.ColorId);

            modelBuilder.Entity<ProductFabric>()
                .HasKey(pf => new { pf.ProductId, pf.FabricId});   

            modelBuilder.Entity<ProductFabric>()
                .HasOne(pf => pf.Product)
                .WithMany(p => p.ProductFabrics)
                .HasForeignKey(pf => pf.ProductId);

            modelBuilder.Entity<ProductFabric>()
                .HasOne(pf => pf.Fabric)
                .WithMany()
                .HasForeignKey(pf => pf.FabricId);

            modelBuilder.Entity<ProductCategory>()
                .HasKey(pct => new { pct.ProductId, pct.CategoryId});

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pct => pct.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pct => pct.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pct => pct.Category)
                .WithMany()
                .HasForeignKey(pct => pct.CategoryId);
        }
    }
}
