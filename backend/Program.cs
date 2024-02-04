using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using backend.Data;

public class Program{
    public static void Main(string[] args){
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope()){
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();

            try{
                dbContext.Database.Migrate();
                Console.WriteLine("Database migration completed successfully.");
            }catch (Exception ex){
                Console.WriteLine($"Error applying migrations: {ex.Message}");
            }
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) => {
                
                config.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>();
            });
}
