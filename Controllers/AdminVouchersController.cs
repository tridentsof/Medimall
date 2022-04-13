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
    public class AdminVouchersController : Controller
    {
        private MedimallEntities db = new MedimallEntities();

        // GET: AdminVouchers
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Voucher> listData = db.Vouchers.ToList();

            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        // GET: AdminVouchers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // GET: AdminVouchers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminVouchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VoucherId,VoucherCode,Percent,VoucherDetail,StartDate,EndDate,StartFrom")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Vouchers.Add(voucher);
                db.SaveChanges();
                TempData["SuccessCreate"] = "Tạo thành công!";
                return RedirectToAction("Index");
            }
            return View(voucher);
        }

        // GET: AdminVouchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // POST: AdminVouchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VoucherId,VoucherCode,Percent,VoucherDetail,StartDate,EndDate,StartFrom")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voucher).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessEdit"] = "Chỉnh sửa voucher thành công";
                return RedirectToAction("Index");
            }
            return View(voucher);
        }

        // GET: AdminVouchers/Delete/5
        public JsonResult Delete(int id)
        {
            bool result = false;
            Voucher voucher = db.Vouchers.Where(a => a.VoucherId == id).SingleOrDefault();

            if (voucher != null)
            {
                db.Vouchers.Remove(voucher);
                TempData["SuccessDelete"] = "Xóa voucher thành công!";
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
