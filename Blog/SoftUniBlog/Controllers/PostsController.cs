using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SoftUniBlog.Models;
using System.Web.Security;

namespace SoftUniBlog.Controllers
{
    [ValidateInput(false)]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var postsWithAuthors = db.Posts.Include(p => p.Author).ToList();

            var currentUserEmail = User.Identity.Name;

            ViewBag.CurrentUserEmail = currentUserEmail;
            return View(postsWithAuthors);
        }

        // GET: Posts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsUserAuthorized = IsUserAuthorized(post);

            return View(post);
        }
    
        
        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "ID,Title,Body")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                post.Date = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            if (!IsUserAuthorized(post))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var authors = db.Users.ToList();

            ViewBag.Authors = authors;
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Body,Author_Id")] Post post)
        {
            if (!IsUserAuthorized(post))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                    
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            if (!IsUserAuthorized(post))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }


            return View(post);
        }

        [Authorize]
        private bool IsUserAuthorized(Post post)
        {
            var context = new ApplicationDbContext();

            var currentUserUsername = User.Identity.Name;
            if (currentUserUsername == null)
            {
                RedirectToAction("Login");
            }
            var currentUser = context.Users.First(a => a.Email == currentUserUsername);

            bool isAuthorized;

            var userIsAuthor = post.Author_Id == currentUser.Id;
            var userIsAdmin = User.IsInRole("Administrators");

            if (userIsAuthor || userIsAdmin)
            {
                isAuthorized = true;
            }
            else
            {
                isAuthorized = false;
            }

            return isAuthorized;
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
