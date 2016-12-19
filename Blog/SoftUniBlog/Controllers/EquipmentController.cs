using SoftUniBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftUniBlog.Controllers
{
    public class EquipmentController : Controller
    {
        // GET: Equipment
        public ActionResult Index(string category)
        {
            Category parsedCategory = ParseCategoryString(category);

            var context = new ApplicationDbContext();

            var equipmentTypes = context.EquipmentTypes.Where(a => a.Category == parsedCategory).ToList();

            ViewBag.Category = parsedCategory;

            return View(equipmentTypes);
        }

        public ActionResult ByType(string id, string category)
        {
            var parsedCategory = ParseCategoryString(category);

            var context = new ApplicationDbContext();

            var equipments = context.Equipments.Where(a => a.Type.Name == id && a.Category == parsedCategory).ToList();

            ViewBag.Type = id;

            return View(equipments);
        }

        private static Category ParseCategoryString(string category)
        {
            return (Category)Enum.Parse(typeof(Category), category);
        }
    }
}