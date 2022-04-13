using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medimall.Helper;
using Medimall.Infrastructure;
using Medimall.Models;

namespace Medimall.Controllers
{
     [CustomAuthenticationAdminFilter]
    public class AdminCategoriesController : Controller
    {
        private MedimallEntities db = new MedimallEntities();

        // GET: AdminCategories
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Category> listData = db.Categories.ToList();

            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }

        // GET: AdminCategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: AdminCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            var idcheck = db.Categories.Any(m => m.CategoryId == category.CategoryId);
            if(idcheck)
            {
                ModelState.AddModelError("CategoryId", "Đã tồn tại mã danh mục");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["SuccessCreate"] = "Tạo danh mục thành công";
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        // GET: AdminCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            string id = this.RouteData.Values["id"].ToString();
            var idcheck = db.Categories.Where(p=>p.CategoryId != id).Any(m => m.CategoryId == category.CategoryId);
            if (idcheck)
            {
                ModelState.AddModelError("CategoryId", "Đã tồn tại mã danh mục");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessEdit"] = "Chỉnh sửa danh mục thành công";
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        // GET: AdminCategories/Delete/5
        public JsonResult DeleteCategory(string id)
        {
            bool result = false;
            Category category = db.Categories.Where(a => a.CategoryId == id).SingleOrDefault();

            if (category != null)
            {
                db.Categories.Remove(category);
                TempData["SuccessDelete"] = "Xóa thành công!";
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
