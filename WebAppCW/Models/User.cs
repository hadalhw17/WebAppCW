using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebAppCW.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            Likes = new List<Like>();
        }
        public String Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<Like> Likes { get; set; }

    }
}