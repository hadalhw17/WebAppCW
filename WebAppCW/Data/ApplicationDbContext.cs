using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppCW.Models;

namespace WebAppCW.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WebAppCW.Models.User> User { get; set; }
        public DbSet<WebAppCW.Models.Post> Post { get; set; }
        public DbSet<WebAppCW.Models.Comment> Comment { get; set; }
        public DbSet<WebAppCW.Models.Like> Likes { get; set; }
    }
}
