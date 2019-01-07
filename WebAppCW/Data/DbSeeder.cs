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
        public static async Task SeedDbAsync(ApplicationDbContext context, UserManager<User> userManager)
        {
            await SeedAdmin(userManager);
            await SeedCustomers(userManager);

            await SeedCustomerClaims(userManager);
        }

        private static readonly string[] customers =
        {
            "Customer1@email.com",
            "Customer2@email.com",
            "Customer3@email.com",
            "Customer4@email.com",
            "Customer5@email.com"
        };
        private async static Task SeedAdmin(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "Member1@email.com",
                Email = "Member1@email.com",
                Name = "Member1",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (await userManager.FindByNameAsync(user.UserName) == null)
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

        private async static Task SeedCustomers(UserManager<User> userManager)
        {
            foreach(var customer in customers)
            {
                User _customer = new User
                {
                    UserName = customer,
                    Email = customer
                };

                if (await userManager.FindByNameAsync(_customer.UserName) == null)
                {
                    userManager.CreateAsync(_customer, "Password123!").Wait();
                }
            }
        }

        private async static Task SeedCustomerClaims(UserManager<User> _userManager)
        {
            var _users = _userManager.Users.ToList();

            foreach (var user in _users)
            {
                var claimList = (await _userManager.GetClaimsAsync(user)).Select(p => p.Type);
                if (!claimList.Contains("IsCommenter"))
                {
                    await _userManager.AddClaimAsync(user, new Claim("IsCommenter", "true"));
                }

                if (!claimList.Contains("IsAdmin"))
                {
                    await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "false"));
                }
            }
        }
    }
}