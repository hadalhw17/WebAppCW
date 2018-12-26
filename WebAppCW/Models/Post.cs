using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppCW.Models
{
    public class Post
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public String PostAuthor { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        public String PostText { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }
    }
}