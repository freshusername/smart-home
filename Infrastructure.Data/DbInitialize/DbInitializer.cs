﻿using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.DbInitialize
{
    public static class DbInitializer
    {

        public static void SeedData(
			UserManager<AppUser> userManager, 
			RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole("Admin"));
            }
         
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                roleManager.CreateAsync(new IdentityRole("User"));
            }
        }

        public static void SeedUsers(UserManager<AppUser> userManager)
        {
            var admin = new AppUser
            {
                Email = "admin@admin.com",
                UserName = "admin@admin.com",             
                PhoneNumber = "+380930000000",
                EmailConfirmed = true
            };
            
            var user = new AppUser
            {
                Email = "user@user.com",
                UserName = "user@user.com",              
                PhoneNumber = "+380930000000",
                EmailConfirmed = true
            };

            if (userManager.FindByNameAsync(admin.Email).Result == null)
            {
                IdentityResult result;
                result = userManager.CreateAsync(admin, "admin12345").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
            }
          
            if (userManager.FindByNameAsync(user.Email).Result == null)
            {
                IdentityResult result;
                result = userManager.CreateAsync(user, "user12345").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, "User").Wait();
            }
        }
	}
}
