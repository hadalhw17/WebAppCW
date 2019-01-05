using System;
using System.Collections.Generic;

namespace WebAppCW.Models
{
    public class PostComments
    {
        public Post Post { get; set; }
        public IList<Comment> Comments{ get; set; }
        public IList<Like> Likes { get; set; }
    }
}
