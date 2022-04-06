using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medimall.Models;

namespace Medimall.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        private MedimallEntities db = new MedimallEntities();
        public ActionResult News()
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());
            var display = db.News.Where(p => p.NewsId == id).ToList();
            return View(display);
        }

        public ActionResult GetCategoryNews()
        {
            var listCategory = db.NewsCategories.ToList();

            return PartialView("GetCategoryNews", listCategory);
        }
    }
}