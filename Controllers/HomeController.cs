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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = db.Accounts.Where(a => a.UserName == model.UserName && a.Password == model.Password).FirstOrDefault();

                if (account != null)
                {
                    Session["UserNameCustomer"] = account.UserName;
                    Session["UserId"] = account.AccountId;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
                    return View(model);
                }
            }
            else
                return View(model);
        }

        public JsonResult SaveVoucher(int id)
        {
            bool result = false;
            try
            {
                var accountId = Session["UserId"].ToString();
                Account userInfor = db.Accounts.Where(m => m.AccountId.ToString() == accountId).SingleOrDefault();
                userInfor.VoucherId = id;

                db.Accounts.Attach(userInfor);
                db.Entry(userInfor).Property(x => x.VoucherId).IsModified = true;
                db.SaveChanges();
                TempData["SuccessMessVoucher"] = "Lưu thành công! Mời bạn vào mục quản lý Voucher để sử dụng";

                result = true;
            }
            catch (Exception)
            {
                throw;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
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

        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;

            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }

            return cart;
        }

        public ActionResult AddToCart(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.ProductId == id);

            if(product != null)
            {
                GetCart().Add(product);
            }

            return RedirectToAction("ShowCart", "Cart");
        }
    }
}