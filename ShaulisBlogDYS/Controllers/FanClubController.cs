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
    public class FanClubController : Controller
    {
        //
        // GET: /FanClub/

        public ActionResult Index()
        {
            using (BlogDBContext context = new BlogDBContext())
            { 
                Fan[] fans = context.Fans.OrderBy(u => u.ID).ToArray<Fan>();
                return View(fans);
            }
        }

        [HttpPost]
        public ActionResult SearchFanByFirstName(string fanNameSearch)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Fan> fans = context.Fans.Where(u => u.FirstName == fanNameSearch).ToList<Fan>();
                return View("FanSearchResult", fans);
            }
        }

        [HttpPost]
        public ActionResult SearchFanByMinYears(int fanYearsSearch)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                List<Fan> fans = context.Fans.Where(u => u.TimeInClub >= fanYearsSearch).ToList<Fan>();
                return View("FanSearchResult", fans);
            }
        }

        public ActionResult CreateNewFan()
        {
            return View();
        }

        public ActionResult EditFan(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Fan currFan = context.Fans.Where(u => u.ID == id).FirstOrDefault<Fan>();
                return View(currFan);
            }
        }

        [HttpPost]
        public ActionResult Create(string firstName, string lastName, bool gender, DateTime dateOfBirth, int timeInClub)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                //int maxId = context.Fans.OrderBy(u => u.ID).Max().ID;
                Fan newFan = new Fan(firstName, lastName, gender, dateOfBirth, timeInClub);
                //context.Entry(newFan).State = System.Data.Entity.EntityState.Added;
                context.Fans.Add(newFan);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Update(int id, string firstName, string lastName, bool gender, DateTime dateOfBirth, int timeInClub)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Fan currFan = context.Fans.Where(u => u.ID == id).FirstOrDefault<Fan>();
                currFan.FirstName = firstName;
                currFan.LastName = lastName;
                currFan.Gender = gender;
                currFan.BirthDate = dateOfBirth;
                currFan.TimeInClub = timeInClub;
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (BlogDBContext context = new BlogDBContext())
            {
                Fan toDelete = context.Fans.Where(u => u.ID == id).FirstOrDefault<Fan>();
                context.Fans.Remove(toDelete);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
