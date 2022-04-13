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
    public class AdminDeliveriesController : Controller
    {
        private MedimallEntities db = new MedimallEntities();

        // GET: AdminDeliveries
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Delivery> listData = db.Deliveries.ToList();

            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        // GET: AdminDeliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // GET: AdminDeliveries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminDeliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeliveryId,DeliveryName,DeliveryPrice")] Delivery delivery)
        {
            var namecheck = db.Deliveries.Any(m => m.DeliveryName == delivery.DeliveryName);
            if (namecheck)
            {
                ModelState.AddModelError("DeliveryName", "Đã tồn tại đơn vị vận chuyển");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Deliveries.Add(delivery);
                    db.SaveChanges();
                    TempData["SuccessCreate"] = "Tạo thành công!";
                    return RedirectToAction("Index");
                }
            }
            return View(delivery);
        }

        // GET: AdminDeliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: AdminDeliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeliveryId,DeliveryName,DeliveryPrice")] Delivery delivery)
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());
            string getnamedelirvery = db.Deliveries.Where(p => p.DeliveryId == id).Select(u => u.DeliveryName).FirstOrDefault();
            var namecheck = db.Deliveries.Where(p=>p.DeliveryName!=getnamedelirvery).Any(m => m.DeliveryName == delivery.DeliveryName);
            if (namecheck)
            {
                ModelState.AddModelError("DeliveryName", "Đã tồn tại đơn vị vận chuyển");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(delivery).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessEdit"] = "Chỉnh sửa đơn vị vận chuyển thành công";
                    return RedirectToAction("Index");
                }
            }
            return View(delivery);
        }

        // GET: AdminDeliveries/Delete/5
        public JsonResult Delete(int id)
        {
            bool result = false;
            Delivery delivery = db.Deliveries.Where(a => a.DeliveryId == id).SingleOrDefault();

            if (delivery != null)
            {
                db.Deliveries.Remove(delivery);
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
