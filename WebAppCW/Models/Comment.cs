using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppCW.Models
{
    public class Comment
    {
        [Required]
        public int CommentID { get; set; }

        [MaxLength(256)]
        public string AuthorName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(200)]
        public string CommentTitle { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(1000)]
        public string CommentText { get; set; }

        [Required]
        public int PostID { get; set; }
    }
}