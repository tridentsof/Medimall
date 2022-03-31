using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medimall.Controllers
{
    public class CartController : Controller
    {
        private MedimallEntities db = new MedimallEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("ShowCart", "Cart");
            }

            Cart cart = Session["Cart"] as Cart;

            return View(cart);
        }

        public ActionResult UpdateQuantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;

            int productId = int.Parse(form["ProductId"]);
            int quantity = int.Parse(form["quantity"]);

            cart.UpdateQuantity(productId, quantity);

            return RedirectToAction("ShowCart", "Cart");
        }

        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.RemoveItem(id);

            return RedirectToAction("ShowCart", "Cart");
        }

        public PartialViewResult BagCart()
        {
            int totalItem = 0;
            Cart cart = Session["Cart"] as Cart;

            if (cart != null)
            {
                totalItem = cart.TotalQuantity();
                ViewBag.QuantityCart = totalItem;
            }

            return PartialView("BagCart");
        }

        public ActionResult DirectCheckOut(FormCollection form)
        {
            try
            {
                Cart cart = Session["Cart"] as Cart;
                List<BillDetail> billingDetail = new List<BillDetail>();
                decimal? totalMoney = 0;
                
                foreach (var item in cart.Items)
                {
                    billingDetail.Add(new BillDetail { 
                        ProductId = item._shopping_product.ProductId,
                        Quantity = item._shopping_quantity,
                        Price = item._shopping_product.Price,
                        Total = item._shopping_product.Price * item._shopping_quantity,
                        ProductName = item._shopping_product.ProductName
                    });

                    totalMoney += item._shopping_product.Price * item._shopping_quantity;
                }

                TempData["BillProduct"] = billingDetail;
                TempData["TotalMoney"] = totalMoney;

                return RedirectToAction("Checkout", "Cart");
            }
            catch (Exception)
            {

                return Content("Error");
            }
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Purchase(FormCollection form)
        {
            try
            {
                var userName = Session["UserNameCustomer"];
                var userInfor = db.Accounts.FirstOrDefault(x => x.UserName == userName.ToString());
                Cart cart = Session["Cart"] as Cart;
                Billing billing = new Billing();

                billing.AccountId = userInfor.AccountId;
                billing.PurchaseDate = DateTime.Now;
                //Can replace if adress in form collection == null
                billing.Address = userInfor.Address;
                billing.DeliveryId = int.Parse(form["delivery-id"]);
                billing.PayId = int.Parse(form["pay-id"]);
                billing.Status = 1;
                db.Billings.Add(billing);

                foreach (var item in cart.Items)
                {
                    BillDetail billDetail = new BillDetail();

                    var product = db.Products.Single(x => x.ProductId == item._shopping_product.ProductId);
                    product.QuantitySold += item._shopping_quantity;
                    product.Quantity -= item._shopping_quantity;

                    billDetail.BillId = billing.BillId;
                    billDetail.ProductId = item._shopping_product.ProductId;
                    billDetail.Price = item._shopping_product.Price;
                    billDetail.Quantity = item._shopping_product.Quantity;

                    db.BillDetails.Add(billDetail);
                }

                db.SaveChanges();
                cart.ClearCart();

                return RedirectToAction("Index", "Home");

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}