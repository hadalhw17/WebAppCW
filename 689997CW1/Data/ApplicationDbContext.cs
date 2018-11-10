using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using _689997CW1.Models;

namespace _689997CW1.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<_689997CW1.Models.User> User { get; set; }
        public DbSet<_689997CW1.Models.Post> Post { get; set; }
    }
}
