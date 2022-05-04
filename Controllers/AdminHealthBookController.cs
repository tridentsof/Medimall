using Medimall.Helper;
using Medimall.Infrastructure;
using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medimall.Controllers
{
    [CustomAuthenticationAdminFilter]
    public class AdminHealthBookController : Controller
    {
        private MedimallEntities db = new MedimallEntities();
        // GET: AdminHealthBook
        public ActionResult Index()
        {
            var healthBook = db.HealthBooks.ToList();
            return View(healthBook);
        }
        public ActionResult GetPaggedData(int pageNumber = 1, int pageSize = 6)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var healthBook = db.HealthBooks.OrderByDescending(p=>p.BookId).ToList();
            var pagedData = Pagination.PagedResult(healthBook, pageNumber, pageSize);

            return Json(pagedData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewDetail(int? id)
        {

            var bookid = db.HealthBooks.Where(p => p.BookId == id).FirstOrDefault();
            var getUserInfo = db.Accounts.Where(p => p.AccountId == bookid.AccountId).FirstOrDefault();

            ViewBag.FullName = getUserInfo.FullName;
            ViewBag.Phone = getUserInfo.Phone;
            ViewBag.Address = getUserInfo.Address;

            DateTime now = DateTime.Now;
            var getAge = DateTime.Now.Year - Convert.ToDateTime(getUserInfo.BirthDay).Year;

            ViewBag.Age = getAge;

            return View(bookid);
        }
        public ActionResult DeleteHealthBook(int id)
        {
            bool result = false;
            HealthBook healthBook = db.HealthBooks.Where(a => a.BookId == id).SingleOrDefault();
            Account account = db.Accounts.Where(p => p.AccountId == healthBook.AccountId).SingleOrDefault();

            if (healthBook != null)
            {
                account.IsHealthCare = false;
                db.HealthBooks.Remove(healthBook);
                TempData["SuccessMess"] = "Xóa thành công!";
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}