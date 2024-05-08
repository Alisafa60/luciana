using System;
using System.Linq;
using backend.Data;

public static class DataSeeder {
   public static void SeedAdmin(AppDbContext context) {
        try {
            Console.WriteLine("Starting seeding process...");

            if (!context.Users.Any(u => u.Role.UserRole == UserRole.Admin)) {
                Console.WriteLine("Admin user does not exist. Creating...");

                var adminRole = new Role { UserRole = UserRole.Admin };
                context.Roles.Add(adminRole);
                context.SaveChanges();
                Console.WriteLine("Admin role created successfully.");

                var adminUser = new User {
                    Email = "ali@admin.com",
                    Password = "adminpassword",
                    FirstName = "Ali",
                    LastName = "Safa",
                    DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Gender = "Male",
                    RoleId = adminRole.Id
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
                Console.WriteLine("Admin user created successfully.");
            } else {
                Console.WriteLine("Admin user already exists.");
            }

            Console.WriteLine("Seeding process completed successfully.");
        } catch (Exception ex) {
            Console.WriteLine($"An error occurred while seeding admin user: {ex.Message}");
            throw;
        }
    }

}

