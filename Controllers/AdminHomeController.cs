using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medimall.Infrastructure;

namespace Medimall.Controllers
{
    //[CustomAuthenticationAdminFilter]
    public class AdminHomeController : Controller
    {
        private MedimallEntities db = new MedimallEntities();
        // GET: AdminHome

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account model)
        {
            if (ModelState.IsValid)
            {
                Account account = db.Accounts.Where(a => a.UserName == model.UserName && a.Password == model.Password).FirstOrDefault();

                if (account != null)
                {
                    Session["UserName"] = account.UserName;

                    return RedirectToAction("Index", "AdminHome");
                }
                else
                {
                    ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
                    return View(model);
                }
            }
            else
                return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserName"] = string.Empty;

            return RedirectToAction("Login", "AdminHome");
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}