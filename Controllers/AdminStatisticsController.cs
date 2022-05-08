using Medimall.Helper;
using Medimall.Infrastructure;
using Medimall.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Medimall.Controllers
{
    [CustomAuthenticationAdminFilter]
    public class AdminStatisticsController : Controller
    {
        // GET: AdminStatistics
        private MedimallEntities db = new MedimallEntities();
        public ActionResult Index()
        {
            var account = db.Accounts.Where(p=>p.Status==1).ToList();
            ViewBag.Account = account.Count();
            var staff = db.Admins.Where(p => p.RoleId == 2).ToList();
            ViewBag.Staff = staff.Count();
            var admin = db.Admins.Where(p => p.RoleId == 1).ToList();
            ViewBag.Admin = admin.Count();
            return View();
        }
        public JsonResult GetSearchingData(string Startday,string Endday )
        {
            db.Configuration.ProxyCreationEnabled = false;
            DateTime FromDay = Convert.ToDateTime(Startday);
            DateTime EndDay = Convert.ToDateTime(Endday);
            var gettotal = db.Billings.Where(p => p.PurchaseDate > FromDay && p.PurchaseDate < EndDay && p.Status==3)
            .DefaultIfEmpty()
            .Sum(p => p.Total);
            ViewBag.Total = gettotal;
            var getbillid = db.Billings.Where(p => p.PurchaseDate > FromDay && p.PurchaseDate < EndDay && p.Status == 3).ToList();

            var getbilldetail = db.BillDetails.ToList();

            List<BillDetail> getalldetail = new List<BillDetail>();

            foreach (var item in getbillid)
            {

                for (int i = 0; i < getbilldetail.Count; i++)
                {
                    if (item.BillId == getbilldetail[i].BillId)
                    { 
                        getalldetail.Add(getbilldetail[i]);
                    }
                }
            }

            List<BillDetail> getRevenue = getalldetail
                .GroupBy(p => p.ProductId)
                .Select(m => new BillDetail
                { 
                    ProductName=m.First().ProductName,
                    Price=m.First().Price,
                    Quantity=m.Sum(c=>c.Quantity),
                }).ToList();

            var result = new { data= getRevenue, total= gettotal };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}