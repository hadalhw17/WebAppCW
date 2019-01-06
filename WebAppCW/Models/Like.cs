using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCW.Models
{
    public class Like
    {
        [Required]
        public int LikeId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string UserName { get; set; }
    }
}
