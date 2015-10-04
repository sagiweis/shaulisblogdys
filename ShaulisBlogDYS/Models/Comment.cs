using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShaulisBlogDYS.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AuthorSiteUrl { get; set; }
        public string CommentText { get; set; }
        public int  PostID { get; set; }
        public Post Post { get; set; }

        public Comment(string Title, string Author, string AuthorSiteUrl, string CommentText, Post CurrentPost)
        {
            this.Author = Author;
            this.Title = Title;
            this.AuthorSiteUrl = AuthorSiteUrl;
            this.CommentText = CommentText;
            this.Post = CurrentPost;
            this.PostID = CurrentPost.ID;
        }

        public Comment()
        {
        }
    }



}