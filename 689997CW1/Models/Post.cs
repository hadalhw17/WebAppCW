using System;
using System.ComponentModel.DataAnnotations;

namespace _689997CW1.Models
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

        public Comment[] Comments { get; set; }
    }
}