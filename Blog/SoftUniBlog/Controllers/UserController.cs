using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SoftUniBlog.Migrations.Models;
using SoftUniBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace SoftUniBlog.Controllers.Admin
{
    [Authorize(Roles = "Administrators")]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        //GET: User/List
        public ActionResult List()
        {
            var users = db.Users.ToList();
            var admins = GetAdminUserNames(users, db);
            ViewBag.Admins = admins;
            return View(users);
        }
        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var admins = new HashSet<string>();
            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, "Administrators"))
                {
                    admins.Add(user.UserName);

                }

            }
            return admins;
        }
        //
        // GET: USer/Edit
        public ActionResult Edit(string id)
        {
            // Validate Id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get user from database
            var user = db.Users.Where(u => u.Id == id).First();
            //Check if user exists
            if (user == null)
            {
                return HttpNotFound();
            }
            //Create a view model
            var viewModel = new EditUserViewModel();
            viewModel.User = user;
            viewModel.Roles = GetUserRoles(user, db);
            //Pass the model to the view
            return View(viewModel);
        }

        private List<Role> GetUserRoles(ApplicationUser user, ApplicationDbContext db)
        {
            // Create user and role managers
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Get all application roles
            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            // For each application role, check if the user has it
            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {
                    role.IsSelected = true;
                }

                userRoles.Add(role);
            }

            // Return a list with all roles
            return userRoles;
        }
        //
        //Post: User/Edit
        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //Get user
                var user = db.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                //if Pass is not empty change password
                if (!string.IsNullOrEmpty(viewModel.Password))
                {
                    var hasher = new PasswordHasher();
                    var passwordHash = hasher.HashPassword(viewModel.Password);
                    user.PasswordHash = passwordHash;
                }
                //user prop
                user.Email = viewModel.User.Email;
                user.FullName = viewModel.User.FullName;
                user.UserName = viewModel.User.Email;
                this.SetUserRoles(user, db, viewModel);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(viewModel);

        }
        private void SetUserRoles(ApplicationUser user, ApplicationDbContext db, EditUserViewModel model)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in model.Roles)
            {
                if (role.IsSelected)
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.IsSelected)
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }
        //
        //GET: User/Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Where(u => u.Id.Equals(id)).First();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);

        }
        //
        //POST: User/Dlete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Where(u => u.Id.Equals(id)).First();
            var userPosts = db.Posts.Where(a => a.Author.Id == user.Id);
            foreach (var post in userPosts)
            {
                db.Posts.Remove(post);
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("List");


        }
    }
}