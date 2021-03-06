﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaulisBlogDYS.Models;

namespace ShaulisBlogDYS.Controllers
{
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/

        public ActionResult Index()
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Post[] posts = context.Posts.OrderBy(u => u.ID).ToArray<Post>();
                for (int index = 0; index < posts.Length; index++)
                {
                    Post currPost = posts[index];
                    posts[index].Comments = context.Comments.Where(u => u.PostID == currPost.ID).ToList<Comment>();
                }

                Dictionary<string, int> statistics = context.Posts.GroupBy(x => new { x.PostDate.Year, x.PostDate.Month, x.PostDate.Day })
                    .Select(g=>new {Key = g.Key, Count=g.Count()})
                    .OrderBy(x=>x.Key.Year)
                    .ThenBy(x=>x.Key.Month)
                    .ThenBy(x=>x.Key.Day)
                    .ToDictionary(p => (p.Key.Day.ToString() + "/" + p.Key.Month.ToString() + "/" + p.Key.Year.ToString()),h=>h.Count);

                PostsStatistics model = new PostsStatistics();
                model.Posts = posts;
                model.Statistics = statistics;

                return View(model);
            }
        }

        public ActionResult AddPost()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Post currPost = context.Posts.Where(u => u.ID == id).FirstOrDefault<Post>();
                currPost.Comments = context.Comments.Where(u => u.PostID == currPost.ID).ToList<Comment>();
                return View(currPost);
            }
        }


        public ActionResult CommentsDetails(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Post currPost = context.Posts.Where(u => u.ID == id).FirstOrDefault<Post>();
                currPost.Comments = context.Comments.Where(u => u.PostID == currPost.ID).ToList<Comment>();
                return View(currPost.Comments);
            }
        }

        public ActionResult EditComment(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Comment currComment = context.Comments.Where(u => u.ID == id).FirstOrDefault<Comment>();
                return View(currComment);
            }
        }

        public ActionResult Edit(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Post currPost = context.Posts.Where(u => u.ID == id).FirstOrDefault<Post>();
                return View(currPost);
            }
        }



        public ActionResult DeleteComment(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Comment toDelete = context.Comments.Where(u => u.ID == id).FirstOrDefault<Comment>();
                context.Comments.Remove(toDelete);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Post toDelete = context.Posts.Where(u => u.ID == id).FirstOrDefault<Post>();
                context.Posts.Remove(toDelete);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(string title, string author, string siteurl, string content, HttpPostedFileBase uploadedImage, string uploadedVideo)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                byte[] image = null;
                MemoryStream target;

                if (uploadedImage != null)
                {
                    target = new MemoryStream();
                    uploadedImage.InputStream.CopyTo(target);
                    image = target.ToArray();
                }

                Post newPost = new Post(author, title, siteurl, content, image, uploadedVideo);
                context.Posts.Add(newPost);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult SearchCommentsByAuthor(string authorNameSearchComment)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Comment> comments = context.Comments.Where(u => u.Author == authorNameSearchComment).ToList<Comment>();
                return View("CommentsSearchResult", comments);
            }
        }

        [HttpPost]
        public ActionResult SearchCommentsByContent(string commentContent)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Comment> comments = context.Comments.Where(u => u.CommentText.Contains(commentContent) == true).ToList<Comment>();
                return View("CommentsSearchResult", comments);
            }
        }

        [HttpPost]
        public ActionResult SearchPostByAuthor(string authorNameSearchPost)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Post> posts = context.Posts.Where(u => u.Author == authorNameSearchPost).ToList<Post>();
                for (var i = 0; i < posts.Count; i++)
                {
                    Post current = posts[i];
                    current.Comments = context.Comments.Where(u => u.PostID == current.ID).ToList<Comment>();
                }
                return View("PostSearchResult", posts);
            }
        }

        [HttpPost]
        public ActionResult SearchPostByDate(DateTime fromDateSearchPost, DateTime toDateSearchPost)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Post> posts = context.Posts.Where(u => u.PostDate.CompareTo(fromDateSearchPost) >= 0 && u.PostDate.CompareTo(toDateSearchPost) <= 0).ToList<Post>();
                for (var i = 0; i < posts.Count; i++)
                {
                    Post current = posts[i];
                    current.Comments = context.Comments.Where(u => u.PostID == current.ID).ToList<Comment>();
                }
                return View("PostSearchResult",posts);
            }
        }

        [HttpPost]
        public ActionResult UpdateComment(int id, string content)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Comment toEdit = context.Comments.Where(u => u.ID == id).FirstOrDefault<Comment>();
                toEdit.CommentText = content;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult SearchPostByCommentContainText(string commentText)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Post> searchResult = context.Posts.Join(context.Comments, p => p.ID, c => c.PostID, (p, c) => new { Post = p, Comment = c }).Where(u => u.Comment.CommentText.Contains(commentText)).Select(x => x.Post).ToList<Post>();
                for (var i = 0; i < searchResult.Count; i++)
                {
                    Post current = searchResult[i];
                    current.Comments = context.Comments.Where(u => u.PostID == current.ID).ToList<Comment>();
                }
                return View("PostSearchResult", searchResult);
            }
        }

        [HttpPost]
        public ActionResult SearchPostByCommentAuthor(string authorName)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Post> searchResult = context.Posts.Join(context.Comments, p => p.ID, c => c.PostID, (p, c) => new { Post = p, Comment = c}).Where(u=>u.Comment.Author == authorName).Select(x=>x.Post).ToList<Post>();
                for (var i = 0; i < searchResult.Count; i++)
                {
                    Post current = searchResult[i];
                    current.Comments = context.Comments.Where(u => u.PostID == current.ID).ToList<Comment>();
                }
                return View("PostSearchResult", searchResult);
            }
        }

        [HttpPost]
        public ActionResult Update(int id, string title, string author, string siteurl, string content, HttpPostedFileBase uploadedImage, string uploadedVideo)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                byte[] image = null;
                MemoryStream target;

                if (uploadedImage != null)
                {
                    target = new MemoryStream();
                    uploadedImage.InputStream.CopyTo(target);
                    image = target.ToArray();
                }

                Post newPost = new Post(author, title, siteurl, content, image, uploadedVideo);
                Post currPost = context.Posts.Where(u => u.ID == id).FirstOrDefault<Post>();
                currPost.Author = newPost.Author;
                currPost.Comments = newPost.Comments;
                currPost.Content = newPost.Content;
                currPost.Image = newPost.Image;
                currPost.PostDate = newPost.PostDate;
                currPost.SiteUrl = newPost.SiteUrl;
                currPost.Title = newPost.Title;
                currPost.Video = newPost.Video;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }
}
