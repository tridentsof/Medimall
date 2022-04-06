using Medimall.Infrastructure;
using Medimall.Models;
using System;
using System.Collections.Generic;
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
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).ToList();
            Billing status = db.Billings.Find(accountId);
            
            return PartialView(displayoder);
        }
        public ActionResult GetWaitOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p=>p.Status==1).ToList();
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
        public ActionResult EditCustomer(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);

            ViewBag.AccountName = account.FullName;
            ViewBag.Birthday = account.BirthDay;
            ViewBag.Phone = account.Phone;
            ViewBag.Email = account.Email;


            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
    }
}