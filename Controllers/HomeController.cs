using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medimall.Models;

namespace Medimall.Controllers
{
    public class HomeController : Controller
    {
        private MedimallEntities db = new MedimallEntities();
        // GET: Home
        public ActionResult Index()
        {
            List<Product> listPD = db.Products.ToList();

            return View();
        }

        public ActionResult GetCategories()
        {
            List<Category> listCate = db.Categories.ToList();
            return PartialView("~/Views/Home/GetCategories.cshtml", listCate);
        }

        public ActionResult GetCategoriesWithImage()
        {
            List<Category> listCate = db.Categories.ToList();
            return PartialView(listCate);
        }

        public ActionResult GetVoucher()
        {
            List<Voucher> listVoucher = db.Vouchers.Where(v => v.EndDate < DateTime.Now).ToList();
            return PartialView(listVoucher);
        }

        public ActionResult GetCovidProductPageOne()
        {
            List<Product> listProduct = db.Products.Where(p => p.IsForCovid == true).OrderBy(p => p.ProductId).Take(5).ToList();
            return PartialView(listProduct);
        }

        public ActionResult GetCovidProductPageTwo()
        {
            List<Product> listProduct = db.Products.Where(p => p.IsForCovid == true).OrderBy(p => p.ProductId).Skip(5).Take(5).ToList();
            return PartialView(listProduct);
        }

        public ActionResult GetCovidProductPageThree()
        {
            List<Product> listProduct = db.Products.Where(p => p.IsForCovid == true).OrderBy(p => p.ProductId).Skip(10).Take(5).ToList();
            return PartialView(listProduct);
        }
    }
}