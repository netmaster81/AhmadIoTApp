using AhmadIoTApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AhmadIoTApp.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var options = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            using (var context = new ApplicationDbContext(options))
            {
                var _userManager = services.GetRequiredService<UserManager<User>>();
                var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                
                var roleNames = new[] { "Admin", "Member" };
                foreach (var name in roleNames)
                {
                    if (await _roleManager.RoleExistsAsync(name)) continue;

                    var role = new IdentityRole { Name = name };
                    var result = await _roleManager.CreateAsync(role);

                    if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                }

                var adminEmail = "hafar.ahmad@gmail.com";

                var foundUser = await _userManager.FindByEmailAsync(adminEmail);

                if (foundUser != null) return;

                var adminUser = new User
                {
                    UserName = "hafar.ahmad@gmail.com",
                    Email = "hafar.ahmad@gmail.com",
                };

                var addUserResult = await _userManager.CreateAsync(adminUser, "BuildMig123!");
                if (!addUserResult.Succeeded) throw new Exception(string.Join("\n", addUserResult.Errors));

                var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, "Admin");
                if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
            
            }


        }
    }
}
