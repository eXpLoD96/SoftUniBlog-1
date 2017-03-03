using SoftUniBlog.Migrations.Models;
using SoftUniBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SoftUniBlog.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Categories
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        //
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: Categories/Create
        [HttpPost]
        public ActionResult Create(Categories category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(category);
        }
        //
        // GET: Categories/Edit
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        //
        // GET: Categories/Edit
        [HttpPost]
        public ActionResult Edit(Categories category)
        {
            if(ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        //
        // GET: Categories/Delete
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        //
        // POST: Categories/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            var categoryPosts = category.Posts.ToList();
            foreach (var post in categoryPosts)
            {
                db.Posts.Remove(post);
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}