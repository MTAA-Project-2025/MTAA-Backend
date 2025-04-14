using Castle.Core.Resource;
using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Helpers
{
    internal static class Utilities
    {
        public static async Task InitializeTestUser(MTAA_BackendDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (await userManager.FindByEmailAsync(UserSettings.Email) != null) return;

            var testUser = new User()
            {
                UserName = UserSettings.UserName,
                Email = UserSettings.Email,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(testUser, UserSettings.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(testUser, UserRoles.User);
                await userManager.SetPhoneNumberAsync(testUser, UserSettings.PhoneNumber);
            }
            await context.SaveChangesAsync();
        }
    }
}
