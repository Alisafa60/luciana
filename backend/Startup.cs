using System;
using backend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
        services.AddControllers();

        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection String: {connectionString}");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {
        // Create a scope to resolve scoped services
        using (var scope = app.ApplicationServices.CreateScope()) {
            var serviceProvider = scope.ServiceProvider;

            // Retrieve the necessary services
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

        app.UseMiddleware<AuthMiddleware>();
        app.UseMiddleware<AdminMiddleware>();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}
