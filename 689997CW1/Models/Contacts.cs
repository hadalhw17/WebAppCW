using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _689997CW1.Models
{
    public class Contacts
    {
        [Required]
        public int Id { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        public String FirstName { get; set; }

        [MinLength(2), MaxLength(50)]
        public String LastName { get; set; }

        [Phone]
        public String PhoneNumber { get; set; }
    }
}


