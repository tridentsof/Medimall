using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medimall.Models;
using System.Linq.Dynamic;
using Medimall.Helper;
using System.IO;
using Medimall.ViewModels;
using Medimall.Infrastructure;

namespace Medimall.Controllers
{
    [CustomAuthenticationAdminFilter]
    public class AdminAccountsController : Controller
    {
        private MedimallEntities db = new MedimallEntities();

        // GET: AdminAccounts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Account> listData = db.Accounts.ToList();

            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }

        // GET: AdminAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }


        // GET: AdminAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountId,UserName,Password,FullName,Phone,Email,Address,BirthDay,Status,ActiveCode,PowerPoint,Photo")] Account account)
        {
            var usernamecheck = db.Accounts.Any(p => p.UserName == account.UserName);
            if(usernamecheck)
            {
                ModelState.AddModelError("UserName", "Đã tồn tại tên đăng nhập");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    if (account != null)
                    {
                        TempData["SuccessMess"] = "Tạo thành công!";
                    }

                    var extension = Path.GetExtension(account.Photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Vendor/img/avatar"));

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    account.Photo.SaveAs(path + account.AccountId + extension);

                    ModelState.Clear();

                    return RedirectToAction("Index");
                }
            }
        
                return View(account);
        }

        // GET: AdminAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);

            if (account.Status == 1)
            {
                ViewBag.Status = "Mở";
                ViewBag.Icon = "fas fa-toggle-on";
            }


            else
            {
                ViewBag.Status = "Đóng";
                ViewBag.Icon = "fas fa-toggle-off";
            }


            ViewBag.AccountName = account.FullName;
            ViewBag.Point = account.PowerPoint;
            ViewBag.Phone = account.Phone;


            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountId,UserName,Password,FullName,Phone,Email,Address,BirthDay,Status,ActiveCode,PowerPoint,Photo")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMess"] = "Lưu thành công!";

                return RedirectToAction("Index");
            }
            return View(account);
        }

        public JsonResult DeleteAccount(int id)
        {
            bool result = false;
            Account account = db.Accounts.Where(a => a.AccountId == id).SingleOrDefault();

            if (account != null)
            {
                db.Accounts.Remove(account);
                TempData["SuccessMess"] = "Xóa thành công!";
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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
