using System;
using backend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
        services.AddDistributedMemoryCache();
        services.AddSession(options => {
            options.IdleTimeout = TimeSpan.FromHours(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddControllers();

        services.AddCors(options => {
            options.AddPolicy("AllowAllOrigins", builder => {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection String: {connectionString}");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IParentCategoryRepository, ParentCategoryRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IParentColorRepository, ParentColorRepository>();
        services.AddScoped<IFabricRepository, FabricRepository>();
        services.AddScoped<IParentFabricRepository, ParentFabricRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<ITexturePatternRepository, TexturePatternRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IAttributeService, AttributesService>();
        services.AddScoped<IProductHistoryRepository, ProductHistoryRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();

        services.AddScoped<LuceneSearchService>(provider => {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var indexDirectoryPath = configuration["LuceneSettings:IndexDirectoryPath"];
            var attributeService = provider.GetRequiredService<IAttributeService>();
            return new LuceneSearchService(connectionString, indexDirectoryPath, attributeService);
        });

        var secretKey = Configuration["JwtSettings:SecretKey"];
        if (secretKey == null) {
            throw new InvalidOperationException("JwtSettings:SecretKey configuration is missing or null.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                };
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {
        using (var scope = app.ApplicationServices.CreateScope()) {
            var serviceProvider = scope.ServiceProvider; 
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            DataSeeder.SeedAdmin(dbContext);
        }

        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        } else {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        logger.LogInformation("Application started.");

        app.UseRouting();
        
        app.UseCors("AllowAllOrigins");

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.Map("/api/admin", adminApp => {
            adminApp.UseMiddleware<AdminMiddleware>();
            adminApp.UseRouting(); 
            adminApp.UseAuthentication();
            adminApp.UseAuthorization();
            adminApp.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        });

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}
