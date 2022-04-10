using Medimall.Infrastructure;
using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Medimall.Controllers
{
    [CustomAuthenticationCustomerFilter]
    public class AccountController : Controller
    {
        // GET: Account
        private MedimallEntities db = new MedimallEntities();
        public ActionResult Account()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayuser = db.Accounts.Where(p => p.AccountId == accountId).ToList();
            return PartialView(displayuser);
        }
        public ActionResult GetAllOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).OrderByDescending(m => m.PurchaseDate).ToList();
            Billing status = db.Billings.Find(accountId);

            return PartialView(displayoder);
        }
        public ActionResult GetWaitOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).OrderByDescending(m => m.PurchaseDate).Where(p => p.Status == 1).ToList();
            return PartialView(displayoder);
        }
        public ActionResult GetDeliOder()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p => p.Status == 2).ToList();
            return PartialView(displayoder);
        }
        public ActionResult GetSuccesOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p => p.Status == 3).ToList();
            return PartialView(displayoder);
        }
        public ActionResult GetCancleOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p => p.Status == 4).ToList();
            return PartialView(displayoder);
        }
        public ActionResult GetAddressOder()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayaddress = db.Billings.Where(p => p.AccountId == accountId).ToList();
            return PartialView(displayaddress);
        }
        public ActionResult Edit()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayuser = db.Accounts.Where(p => p.AccountId == accountId).ToList();
            return PartialView("Edit", displayuser);
        }
        [HttpPost]
        public ActionResult UpdateCustomer(Account customer)
        {
            var accountId = int.Parse(Session["UserId"].ToString()); 
            Account account= (from c in db.Accounts
                              where c.AccountId == accountId
                              select c).FirstOrDefault();
            if (account != null)
            {
                account.FullName = customer.FullName;
                account.BirthDay = customer.BirthDay;
                account.Phone = customer.Phone;
                account.Email = customer.Email;
                account.Gender = customer.Gender;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        public ActionResult Voucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var voucher = db.Vouchers.Where(p => p.AccountId == accountId).ToList();
            return PartialView(voucher);
        }
        public ActionResult ForceVoucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            int? bill = db.Billings.Where(u => u.AccountId == accountId).Select(u => u.BillId).FirstOrDefault();


            int? voucherid = db.BillDetails.Where(u => u.BillId == bill).Select(u => u.VoucherId).FirstOrDefault();

            var voucher = db.Vouchers.Where(p => p.AccountId == accountId).Where(p=>p.EndDate > DateTime.Now).Where(p=>p.VoucherId != voucherid).ToList();
            return PartialView(voucher);
        }
        public ActionResult ExpireVoucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            int? bill = db.Billings.Where(u => u.AccountId == accountId).Select(u => u.BillId).FirstOrDefault();


            int? voucherid = db.BillDetails.Where(u => u.BillId == bill).Select(u => u.VoucherId).FirstOrDefault();

            var voucher = db.Vouchers.Where(p => p.AccountId == accountId).Where(p => p.EndDate < DateTime.Now).Where(p => p.VoucherId != voucherid).ToList();
            return PartialView(voucher);
        }
        public ActionResult UsedVoucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            int ? bill = db.Billings.Where(u => u.AccountId == accountId).Select(u => u.BillId).FirstOrDefault();


            int ? voucherid = db.BillDetails.Where(u => u.BillId == bill).Select(u => u.VoucherId).FirstOrDefault();

            var voucher = db.Vouchers.Where(p => p.VoucherId == voucherid).ToList();
             return PartialView(voucher);
        }
    }
}