using System;
using System.Collections.Generic;

namespace WebAppCW.Models
{
    public class PostComment
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
        public List<Like> Likes { get; set; }
    }
}
