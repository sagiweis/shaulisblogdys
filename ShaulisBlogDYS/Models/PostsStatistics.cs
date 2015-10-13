using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaulisBlogDYS.Models
{
    public class PostsStatistics
    {
        public Post[] Posts;
        public Dictionary<string, int> Statistics;
    }
}