using System;

namespace WebAppCW.Models
{
    public class PostComment
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
    }
}
