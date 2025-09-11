using E_commerce.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Infrastructure.Identity
{
    public class IdentitySeeder
    {
        public static async Task SeedRolesAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = { "User", "Manager", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed default Manager account
            var managerEmail = "manager@ecomdemo.com";
            var managerUser = await userManager.FindByEmailAsync(managerEmail);

            if (managerUser == null)
            {
                var newManager = new AppUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FirstName = "Director",
                    LastName = "Manager",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newManager, "P@ssword123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newManager, "Manager");
                }
            }


            // Seed default Admin account
            var adminEmail = "admin@ecomdemo.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Dev",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "P@sswordAdmin123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Manager");
                }
            }

        }
    }
}