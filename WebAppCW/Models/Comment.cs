using System;

namespace WebAppCW.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        
        public String AuthorName { get; set; }

        public String CommentText { get; set; }

        public int PostID { get; set; }
    }
}