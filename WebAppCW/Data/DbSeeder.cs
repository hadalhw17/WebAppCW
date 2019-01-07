//Created by Aleksandr Slobodov, student number 689997

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

        // List of customer emails and usernames to be seeded.
        private static readonly string[] customers =
        {
            "Customer1@email.com",
            "Customer2@email.com",
            "Customer3@email.com",
            "Customer4@email.com",
            "Customer5@email.com"
        };

        // Creates blog admin, assigns administrator claims.
        private async static Task SeedAdmin(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "Member1@email.com",
                Email = "Member1@email.com",
                Name = "Member1",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // If admin does not exist, create him.
            // Skip this step otherwise.
            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                userManager.CreateAsync(user, "Password123!").Wait();
            }

            // Check if admin has all of the admin claims.
            // If not then assign them.
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

        // Create all of the cusmers from the customer list.
        private async static Task SeedCustomers(UserManager<User> userManager)
        {
            // Iterate over customner names in the array.
            foreach(var customer in customers)
            {
                // Creates user entity.
                User _customer = new User
                {
                    UserName = customer,
                    Email = customer
                };

                // Checks if user is already created.
                // If yes then skip this step.
                if (await userManager.FindByNameAsync(_customer.UserName) == null)
                {
                    // Creates user with default password.
                    userManager.CreateAsync(_customer, "Password123!").Wait();
                }
            }
        }

        // Seeds claims for customers in the database.
        // Claims for customers are:
        // IsCommenter = true
        // IsAdmin = false
        private async static Task SeedCustomerClaims(UserManager<User> _userManager)
        {
            // Gets list of all users.
            var _users = _userManager.Users.ToList();

            // Iterate over them.
            foreach (var user in _users)
            {
                // Get list of all claims for users.
                var claimList = (await _userManager.GetClaimsAsync(user)).Select(p => p.Type);

                // Check if claims wera already assigned.
                // If yes, then skip this step.
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