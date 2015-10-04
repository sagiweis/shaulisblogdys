/*
 * Sagi Weisman - 312490030
 * Yasmin Karlin - 308417138
 */

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ShaulisBlogDYS.Models;

namespace ShaulisBlogDYS
{
    public class BlogDBContext : DbContext
    {
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogDBContext()
            : base("DefaultConnection")
        { }
    }
}