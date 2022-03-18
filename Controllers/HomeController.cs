using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medimall.Models;

namespace Medimall.Controllers
{
    public class HomeController : Controller
    {
        MedimallEntities db = new MedimallEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}