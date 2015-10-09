using System;
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
                return View(posts);
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
        public ActionResult Create(string title, string author, string siteurl, string content, HttpPostedFileBase uploadedImage, HttpPostedFileBase uploadedVideo)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                byte[] image = null;
                byte[] video = null;
                MemoryStream target;

                if (uploadedImage != null)
                {
                    target = new MemoryStream();
                    uploadedImage.InputStream.CopyTo(target);
                    image = target.ToArray();
                }

                if (uploadedVideo != null)
                {
                    target = new MemoryStream();
                    uploadedVideo.InputStream.CopyTo(target);
                    video = target.ToArray();
                }
                
                Post newPost = new Post(author, title, siteurl, content, image, video);
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
        public ActionResult Update(int id, string title, string author, string siteurl, string content, HttpPostedFileBase uploadedImage, HttpPostedFileBase uploadedVideo)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                byte[] image = null;
                byte[] video = null;
                MemoryStream target;

                if (uploadedImage != null)
                {
                    target = new MemoryStream();
                    uploadedImage.InputStream.CopyTo(target);
                    image = target.ToArray();
                }

                if (uploadedVideo != null)
                {
                    target = new MemoryStream();
                    uploadedVideo.InputStream.CopyTo(target);
                    video = target.ToArray();
                }

                Post newPost = new Post(author, title, siteurl, content, image, video);
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
