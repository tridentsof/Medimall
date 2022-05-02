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
            var user = db.Accounts.Where(p => p.AccountId == accountId).FirstOrDefault();
            var pointToVIP = 2000000 - user.PowerPoint.GetValueOrDefault();
            decimal useablePoint = 0;
            TempData["PointToVIP"] = String.Format("{0:0,0}",pointToVIP);

            if(pointToVIP < 0)
            {
                useablePoint = Math.Abs(pointToVIP);
                TempData["UseablePoint"] = String.Format("{0:0,0}", useablePoint);
            }
            else
            {
                useablePoint = 0;
                TempData["UseablePoint"] = String.Format("{0:0,0}", useablePoint);
            }
            
            return PartialView(user);
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
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p => p.Status == 0).ToList();
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
            var displayuser = db.Accounts.Where(p => p.AccountId == accountId).FirstOrDefault();
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
        public ActionResult OderDetail()
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());
            var diplasydetail = db.BillDetails.Where(p => p.BillId == id).ToList();
            return PartialView(diplasydetail);
        }
        public ActionResult OderDetailHeader()
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());
            var diplasydetail = db.Billings.Where(p => p.BillId == id).FirstOrDefault();
            return PartialView(diplasydetail);
        }
        public ActionResult OderDetailTotal()
        {
            int  id = int.Parse(this.RouteData.Values["id"].ToString());

            List<BillDetail> billDetails = db.BillDetails.Where(u => u.BillId == id).ToList();
            decimal? Total = 0;
            foreach(var item in billDetails)
            {
                for (int i = 0; i < billDetails.Count; i++)
                {
                    Total = Total+(item.Price * item.Quantity);
                    i++;
                }
            }    
             int ? getiddeli = db.Billings.Where(u => u.BillId == id).Select(u => u.DeliveryId).FirstOrDefault();

            decimal DeliPrice = (decimal)db.Deliveries.Where(p => p.DeliveryId == getiddeli).Select(p => p.DeliveryPrice).FirstOrDefault();

            int? voucherid = db.BillDetails.Where(u => u.BillId == id).Select(p => p.VoucherId).FirstOrDefault();
            decimal? PercentVoucher = 0;
            if (voucherid !=null)
            {
                PercentVoucher = db.Vouchers.Where(p => p.VoucherId == voucherid).Select(p => p.Percent).FirstOrDefault();
            }
            decimal? TotalVoucher = (Total * PercentVoucher / 100);

            ViewBag.TotalVoucher = TotalVoucher;

            ViewBag.Total = Total;
            ViewBag.PriceDeli = DeliPrice;

            decimal ? SumTotal = Total + DeliPrice-TotalVoucher;
            ViewBag.Sumtotal = SumTotal;
            var diplasydetail = db.BillDetails.Where(p => p.BillId == id).ToList();
            return PartialView(diplasydetail);
        }
        public JsonResult UpdatePassword(string oldPass,string newPass) 
        {
            db.Configuration.ProxyCreationEnabled = false;
            var accountId = int.Parse(Session["UserId"].ToString());
            var checkvalid = db.Accounts.Any(p => p.AccountId == accountId && p.Password == oldPass);
            Account account = (from c in db.Accounts
                               where c.AccountId == accountId
                               select c).FirstOrDefault();
            if(checkvalid)
            {
                if (account != null)
                {
                    account.Password = newPass;
                    db.SaveChanges();
                    return Json(true);
                }
            }   
            return Json(false);
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
    }
}