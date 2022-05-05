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
    public class AdminCustomerVipController : Controller
    {
        private MedimallEntities db = new MedimallEntities();
        // GET: AdminCustomerVip
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var customerVip = db.Accounts.Where(p => p.IsVIP == true).ToList();
            var pagedData = Pagination.PagedResult(customerVip, pageNumber, pageSize);

            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int ? id)
        {
            var getVipId = db.Accounts.Where(p => p.AccountId == id).FirstOrDefault();
            return View(getVipId);
        }

        public JsonResult UpdatePoint(Account data,int? id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Account account = (from c in db.Accounts
                               where c.AccountId == id
                               select c).FirstOrDefault();
            if (account != null)
            {
                account.PowerPoint = data.PowerPoint;
                account.UsedPoint = data.UsedPoint;
                db.SaveChanges();
                TempData["SuccessUpdate"] = "Cập nhật thành công";
                return Json(true);
            }
                return Json(false);
        }
        public ActionResult BlockVip(int id)
        {
            bool result = false;
            Account account = db.Accounts.Where(p => p.AccountId == id).SingleOrDefault();

            if (account != null)
            {
                account.IsVIP = false;
                account.PowerPoint = 0;
                account.UsedPoint = 0;
                TempData["SuccessBlock"] = "Khóa thành công!";
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}