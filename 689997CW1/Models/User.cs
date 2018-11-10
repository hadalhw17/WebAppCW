using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace _689997CW1.Models
{
    public class User:IdentityUser
    {
        public String Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Post[] UserPosts { get; set; }

    }
}