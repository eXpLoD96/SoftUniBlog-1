﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftUniBlog.Controllers
{
    public class CommentBoxController : Controller
    {
        // GET: CommentBox
        public ActionResult Index()
        {
            return View();
        }
    }
}