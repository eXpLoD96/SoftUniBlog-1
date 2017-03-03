using SoftUniBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftUniBlog.Controllers
{
    public class CommentBoxController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: CommentBox
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create([Bind(Include = "Name,Content,Email")] Comment comment, int po1stId)
        {
            //Post post = db.Posts.Find(postId);
            //if (post == null)
            //{

            //}

            //comment.Post = post;
            //db.SaveChanges();
            return View();
        }
    }
}