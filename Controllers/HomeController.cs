using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medimall.Infrastructure;
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

        [HttpPost]
        public ActionResult ListProductSearch(string searchString)
        {
            var product = db.Products.Where(m => string.IsNullOrEmpty(searchString)
                                            || m.ProductName.ToUpper().Contains(searchString.ToUpper().Trim())
                                            || m.Category.CategoryName.ToUpper().Contains(searchString.ToUpper().Trim())
                                            || m.UsesFor.ToUpper().Contains(searchString.ToUpper().Trim())).ToList();

            ViewBag.ResultCount = product.Count();
            return View("~/Views/Home/ListProduct.cshtml", product);
        }

        public ActionResult ProductSearch()
        {
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
                Account account = db.Accounts.Where(a => a.UserName == model.UserName && a.Password == model.Password && a.Status == 1).FirstOrDefault();

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

        [HttpPost]
        public JsonResult LoginAjax(string userName,string passWord)
        {
            bool result = false;
            Account account = db.Accounts.Where(a => a.UserName == userName && a.Password == passWord && a.Status == 1).FirstOrDefault();
            if (account != null)
            {
                Session["UserNameCustomer"] = account.UserName;
                Session["UserId"] = account.AccountId;
                result = true;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthenticationCustomerFilter]
        public JsonResult SaveVoucher(int id)
        {
            bool result = false;
            try
            {
                var accountId = Session["UserId"].ToString();

                var voucher = db.Vouchers.Where(m => m.VoucherId == id).FirstOrDefault();
                voucher.AccountId = int.Parse(accountId);

                db.Vouchers.Attach(voucher);
                db.Entry(voucher).Property(x => x.AccountId).IsModified = true;
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
            List<Voucher> listVoucher = db.Vouchers.Where(v => v.EndDate > DateTime.Now).ToList();
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

            if (product != null)
            {
                GetCart().Add(product);
            }

            return RedirectToAction("ShowCart", "Cart");
        }
        public ActionResult GetSkinCareProductPageOne()
        {
            List<Product> listProduct = db.Products.Where(p => p.CategoryId == "SD01").OrderBy(p => p.ProductId).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetSkinCareProductPageTwo()
        {
            List<Product> listProduct = db.Products.Where(p => p.CategoryId == "SD01").OrderBy(p => p.ProductId).Skip(5).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetSkinCareProductPageThree()
        {
            List<Product> listProduct = db.Products.Where(p => p.CategoryId == "SD01").OrderBy(p => p.ProductId).Skip(10).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetBestSaleProductPageOne()
        {
            List<Product> listProduct = db.Products.Where(p => p.QuantitySold > 5).OrderBy(p => p.ProductId).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetBestSaleProductPageTwo()
        {
            List<Product> listProduct = db.Products.Where(p => p.QuantitySold > 5).OrderBy(p => p.ProductId).Skip(5).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetBestSaleProductPageThree()
        {
            List<Product> listProduct = db.Products.Where(p => p.QuantitySold > 5).OrderBy(p => p.ProductId).Skip(10).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetNewProductPageOne()
        {
            List<Product> listProduct = db.Products.OrderByDescending(p => p.ProductId).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetNewProductPageTwo()
        {
            List<Product> listProduct = db.Products.OrderByDescending(p => p.ProductId).Skip(5).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetNewProductPageThree()
        {
            List<Product> listProduct = db.Products.OrderByDescending(p => p.ProductId).Skip(10).Take(5).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetNewsPageOne()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 1).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetNewsPageTwo()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 2).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetNewsPageThree()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 3).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetNewsPageFour()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 4).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetNewsPageFive()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 5).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetNewsPageSix()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 6).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetNewsPageSeven()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 7).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetNewsPageEight()
        {
            List<News> listnews = db.News.Where(p => p.NewsCategoryId == 8).OrderByDescending(p => p.NewsCategoryId).Take(5).ToList();
            return PartialView(listnews);
        }
        public ActionResult GetDetailProductPage()
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());
            string categoryid = db.Products.Where(p => p.ProductId == id).Select(u => u.CategoryId).FirstOrDefault();

            string categoryname = db.Categories.Where(p => p.CategoryId == categoryid).Select(u => u.CategoryName).FirstOrDefault();

            ViewBag.CategoryName = categoryname;

            var productdetail = db.Products.Where(p => p.ProductId == id).FirstOrDefault();
            return View(productdetail);
        }
        public ActionResult GetDelivery()
        {
            var delivery = db.Deliveries.ToList();
            return PartialView(delivery);
        }
        public ActionResult SameProductPartial()
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());

            string getcategoryid = db.Products.Where(p => p.ProductId == id).Select(u => u.CategoryId).FirstOrDefault();

            var getsameproduct = db.Products.Where(u => u.CategoryId == getcategoryid).Where(p=>p.ProductId != id).ToList();

            return PartialView(getsameproduct);
        }
        public ActionResult ViewAllCovidProduct()
        {
            List<Product> listProduct = db.Products.Where(p => p.IsForCovid == true).OrderBy(p => p.ProductId).ToList();
            return PartialView(listProduct);
        }
        public ActionResult ViewBestSaleProduct()
        {
            List<Product> listProduct = db.Products.Where(p => p.QuantitySold > 5).OrderBy(p => p.ProductId).ToList();
            return PartialView(listProduct);
        }
        public ActionResult ViewNewProduct()
        {
            List<Product> listProduct = db.Products.OrderByDescending(p => p.ProductId).ToList();
            return PartialView(listProduct);
        }
        public ActionResult ViewSkinCareProduct()
        {
            List<Product> listProduct = db.Products.Where(p => p.CategoryId == "SD01").OrderBy(p => p.ProductId).ToList();
            return PartialView(listProduct);
        }
        public ActionResult ViewProductForCategory()
        {
            string id = this.RouteData.Values["id"].ToString();
            ViewBag.categoryname = id;
            string categoryid = db.Categories.Where(p => p.CategoryName == id).Select(u => u.CategoryId).FirstOrDefault();
            List<Product> listProduct = db.Products.Where(p => p.CategoryId == categoryid).ToList();
            return PartialView(listProduct);
        }
        public ActionResult GetCategoryName()
        {
            List<Category> categories = db.Categories.ToList();
            return PartialView(categories);
        }
    }
}