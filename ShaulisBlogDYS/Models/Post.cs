﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ShaulisBlogDYS.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string SiteUrl { get; set; }
        public DateTime PostDate { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public string Video { get; set; }
        public List<Comment> Comments { get; set; }

        public Post(string Author, string Title, string SiteUrl, string Content, byte[] Image, string Video)
        {
            this.ID = ID;
            this.Author = Author;
            this.Title = Title;
            this.SiteUrl = SiteUrl;
            this.PostDate = DateTime.Now;
            this.Content = Content;
            this.Comments = new List<Comment>();
            this.Image = Image;
            this.Video = Video;
        }

        public Post()
        {
             
        }
    }
}