using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Medimall.Helper;
using Medimall.Models;
using Medimall.Infrastructure;

namespace MedimallAdmin.Controllers
{
    [CustomAuthenticationAdminFilter]
    public class AdminBillingsController : Controller
    {
        private MedimallEntities db = new MedimallEntities();

        // GET: AdminBillings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listData = db.Billings.ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);

            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSearchingData(string SearchBy, string SearchValue)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Billing> listBilling = new List<Billing>();

            if (SearchBy == "Id")
            {
                try
                {
                    listBilling = db.Billings.Where(p => p.BillId.ToString().Equals(SearchValue) || SearchValue == null).ToList();
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} không phải là Id hợp lệ", SearchValue);
                }

                return Json(listBilling, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    listBilling = db.Billings.Where(p => p.AccountId.ToString().Equals(SearchValue) || SearchValue == null).ToList();
                }
                catch (Exception)
                {

                    throw;
                }

                return Json(listBilling, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteBilling(int id)
        {
            bool result = false;
            Billing billing = db.Billings.Where(a => a.BillId == id).SingleOrDefault();

            if (billing != null)
            {
                var lstBillDetail = db.BillDetails.Where(m => m.BillId == id).ToList();
                foreach (var item in lstBillDetail)
                {
                    db.BillDetails.Remove(item);
                }

                db.Billings.Remove(billing);
                TempData["SuccessMess"] = "Xóa thành công!";
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillingDetail()
        {
            var billId = (int)TempData["BillId"];

            var billDetail = db.BillDetails.Where(m => m.BillId == billId).ToList();
            return PartialView("BillingDetail",billDetail);
        }

        // GET: AdminBillings/Edit/5
        public ActionResult EditBilling(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billing billing = db.Billings.Find(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            TempData["BillId"] = billing.BillId;
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "UserName", billing.AccountId);
            ViewBag.DeliveryId = new SelectList(db.Deliveries, "DeliveryId", "DeliveryName", billing.DeliveryId);
            ViewBag.PayId = new SelectList(db.Payments, "PayId", "PayMethod", billing.PayId);
            return View(billing);
        }

        // POST: AdminBillings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBilling([Bind(Include = "BillId,AccountId,PurchaseDate,Address,DeliveryId,Total,PayId,Status,Phone,UserName")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billing).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMess"] = "Lưu thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "AccountId", "UserName", billing.AccountId);
            ViewBag.DeliveryId = new SelectList(db.Deliveries, "DeliveryId", "DeliveryName", billing.DeliveryId);
            ViewBag.PayId = new SelectList(db.Payments, "PayId", "PayMethod", billing.PayId);
            return View(billing);
        }

        // GET: AdminBillings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billing billing = db.Billings.Find(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }

        // POST: AdminBillings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Billing billing = db.Billings.Find(id);
            db.Billings.Remove(billing);
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
