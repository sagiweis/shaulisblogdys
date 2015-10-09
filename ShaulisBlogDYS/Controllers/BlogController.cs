/*
 * Sagi Weisman - 312490030
 * Yasmin Karlin - 308417138
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaulisBlogDYS.Models;

namespace ShaulisBlogDYS.Controllers
{
    public class BlogController : Controller
    {
        //
        // GET: /Blog/

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

        [HttpPost]
        public ActionResult AddComment(int id,string title,string author,string siteurl,string content)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Post relatedPost = context.Posts.Where(u => u.ID == id).FirstOrDefault<Post>();
                Comment comment = new Comment(title, author, siteurl, content, relatedPost);
                context.Comments.Add(comment);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult AdvancedSearchPost(string titleSearchPost, string authorNameSearchPost, DateTime fromDateSearchPost, DateTime toDateSearchPost)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Post> searchResult = context.Posts.Where(u => u.Author.Contains(authorNameSearchPost) && u.Title.Contains(titleSearchPost) && u.PostDate.CompareTo(fromDateSearchPost) >= 0 && u.PostDate.CompareTo(toDateSearchPost) <= 0).ToList<Post>();
                for (var i = 0; i < searchResult.Count; i++)
                {
                    Post current = searchResult[i];
                    current.Comments = context.Comments.Where(u => u.PostID == current.ID).ToList<Comment>();
                }
                return View("PostSearchResult", searchResult);
            }
        }
    }
}
