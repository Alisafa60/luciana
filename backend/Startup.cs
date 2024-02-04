using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.Extensions.Logging;

public class Startup
{
    public Startup(IConfiguration configuration){
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services){
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection String: {connectionString}");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger){
        if (env.IsDevelopment()){
            app.UseDeveloperExceptionPage();
        }else{
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        logger.LogInformation("Application started.");

        app.UseRouting();

        app.UseCors("AllowAllOrigins"); 

        app.UseEndpoints(endpoints =>{
            endpoints.MapControllers();
        });
    }
}
