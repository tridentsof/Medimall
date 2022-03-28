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
                
                foreach (var item in cart.Items)
                {
                    billingDetail.Add(new BillDetail { 
                        ProductId = item._shopping_product.ProductId,
                        Quantity = item._shopping_quantity,
                        Price = item._shopping_product.Price,
                        Total = item._shopping_product.Price * item._shopping_quantity,
                        ProductName = item._shopping_product.ProductName
                    });
                }

                TempData["BillProduct"] = billingDetail;

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
    }
}