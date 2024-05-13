using System;
using System.Linq;
using backend.Data;
using backend.Helpers;

public static class DataSeeder {
   public static void SeedAdmin(AppDbContext context) {
    try {
        Console.WriteLine("Starting seeding process...");

        var existingAdmin = context.Users.FirstOrDefault(u => u.Role.UserRole == UserRole.Admin);
        if (existingAdmin != null) {
            Console.WriteLine("Admin user already exists.");
            return; 
        }

        Console.WriteLine("Admin user does not exist. Creating...");

        var adminRole = context.Roles.FirstOrDefault(r => r.Id == 1); 

        if (adminRole == null) {
            Console.WriteLine("Admin role not found. Ensure it's seeded correctly.");
            return;
        }

        var adminPassword = "adminpassword"; 
        string hashedPassword = PasswordHasher.HashPassword(adminPassword);

        var adminUser = new User {
            Email = "ali1@admin.com",
            Password = hashedPassword,
            FirstName = "Ali",
            LastName = "Safa",
            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Gender = "Male",
            RoleId = adminRole.Id 
        };
        context.Users.Add(adminUser);
        context.SaveChanges();
        Console.WriteLine("Admin user created successfully.");

        Console.WriteLine("Seeding process completed successfully.");
    } catch (Exception ex) {
        Console.WriteLine($"An error occurred while seeding admin user: {ex.Message}");
        throw;
    }
}

}
