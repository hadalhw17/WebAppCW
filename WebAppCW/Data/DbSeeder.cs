using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCW.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebAppCW.Data
{
    public static class DbSeeder
    {

        private static readonly string[] _roles =
        {
            "Blogger",
            "Commenter"
        };
         public static async Task SeedDbAsync(ApplicationDbContext context, UserManager<User> userManager)
         {
            await SeedUser(userManager);
            //await SeedClaims(userManager);
         }

         private async static 
         Task
SeedUser(UserManager<User> userManager)
         {
            User user = new User
            {
                UserName = "AlexAdmin",
                Email = "alex@email.com",
                Name = "Alex",
                SecurityStamp = Guid.NewGuid().ToString()
             };

            if(await userManager.FindByNameAsync(user.UserName) == null)
            {
                userManager.CreateAsync(user, "Password123!").Wait();
            }


            var claimList = (await userManager.GetClaimsAsync(user)).Select(p => p.Type);
            if (!claimList.Contains("IsAdmin"))
            {
                await userManager.AddClaimAsync(user, new Claim("IsAdmin", "true"));
            }

            if (!claimList.Contains("IsCommenter"))
            {
                await userManager.AddClaimAsync(user, new Claim("IsCommenter", "true"));
            }
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach(var role in _roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    var create = await roleManager.CreateAsync(new IdentityRole(role));
                    if(!create.Succeeded)
                    {
                        throw new Exception("Failed to create role " + role);
                    }
                }
            }
        }
        //private async static Task SeedClaims(UserManager<User> _userManager)
        //{
        //    var _users = _userManager.Users.ToList();

        //    foreach(var user in _users)
        //    {
        //        var claimList = (await _userManager.GetClaimsAsync(user)).Select(p => p.Type);
        //        if (!claimList.Contains("IsCommenter"))
        //        {
        //            await _userManager.AddClaimAsync(user, new Claim("IsCommenter", "true"));
        //        }

        //        if (!claimList.Contains("IsAdmin"))
        //        {
        //            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "false"));
        //        }
        //    }
        //}
    }
}