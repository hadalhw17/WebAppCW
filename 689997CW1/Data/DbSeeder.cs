using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _689997CW1.Models;
using Microsoft.AspNetCore.Identity;

namespace _689997CW1.Data
{
    public static class DbSeeder
    {
         public static void SeedDb(ApplicationDbContext context, UserManager<User> userManager)
         {
             SeedContacts(context);
             SeedUser(userManager);
         }

         private static void SeedUser(UserManager<User> userManager)
         {
            User user = new User
             {
                 UserName = "alex@email.com",
                 Email = "alex@email.com"
             };

             userManager.CreateAsync(user, "Password123!").Wait();
         }

         private static void SeedContacts(ApplicationDbContext context)
         {
             //Seeding the database here
             context.Database.EnsureCreated();
             context.User.Add(
                 new User() { Name = "Alex" }
             );
             context.SaveChanges();
         }
    }
}