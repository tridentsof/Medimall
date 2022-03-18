using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medimall.Models;
using Medimall.Helper;
using Medimall.ViewModels;
using System.IO;

namespace Medimall.Controllers
{
    public class AdminProductsController : Controller
    {
        private MedimallEntities db = new MedimallEntities();

        // GET: AdminProducts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaggedData(int pageNumber = 1,int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Product> listData = db.Products.ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);

            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSearchingData(string SearchBy, string SearchValue)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Product> listProduct = new List<Product>();

            if(SearchBy == "Id")
            {
                try
                {
                    listProduct = db.Products.Where(p => p.CategoryId == SearchValue || SearchValue == null).ToList();
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} không phải là danh mục hợp lệ",SearchValue);
                }

                return Json(listProduct, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    listProduct = db.Products.Where(p => p.ProductName.Contains(SearchValue) || SearchValue == null).ToList(); 
                }
                catch (Exception)
                {

                    throw;
                }

                return Json(listProduct, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteProduct(int id)
        {
            bool result = false;
            Product product = db.Products.Where(a => a.ProductId == id).SingleOrDefault();

            if (product != null)
            {
                db.Products.Remove(product);
                TempData["SuccessMess"] = "Xóa thành công!";
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: AdminProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: AdminProducts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,CategoryId,ProductName,UsesFor,Ingredient,Price,Quantity,QuantitySold,Photo")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();

                if (product != null)
                {
                    TempData["SuccessMess"] = "Tạo thành công!";
                }

                var extension = Path.GetExtension(product.Photo.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/products/"));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                product.Photo.SaveAs(path + product.ProductId + extension);

                ModelState.Clear();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: AdminProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,CategoryId,ProductName,UsesFor,Ingredient,Price,Quantity,QuantitySold,Photo")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: AdminProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
