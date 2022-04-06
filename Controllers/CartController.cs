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
            var quantityLeft = db.Products.Where(m => m.ProductId == productId).FirstOrDefault().Quantity;

            if(quantityLeft > quantity)
            {
                cart.UpdateQuantity(productId, quantity);
                return RedirectToAction("ShowCart", "Cart");
            }
            else
            {
                var alert = String.Format("Xin lỗi quý khách! số lượng sản phẩm còn lại trong kho: {0}",quantityLeft);
                TempData["ErrorQuantity"] = alert;
                return RedirectToAction("ShowCart", "Cart");
            }
        }

        public ActionResult ApplyVoucher(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;

            int idVoucher = int.Parse(form["voucher"]);
            var voucher = db.Vouchers.Where(m => m.VoucherId == idVoucher).First();
            decimal startFrom = voucher.StartFrom.GetValueOrDefault();
            int percentDiscount = voucher.Percent.GetValueOrDefault();
            
            if(cart.TotalMoney() < Decimal.ToDouble(startFrom))
            {
                TempData["ErrorVoucher"] = "Giá trị đơn hàng chưa đủ để áp dụng khuyến mãi!";
                return RedirectToAction("ShowCart", "Cart");
            }
            else
            {
                double discountPrice = cart.TotalMoney() * (100 - percentDiscount) / 100;
                TempData["DiscountPrice"] = discountPrice;
                TempData["SuccessVoucher"] = "Áp dụng thành công!";
                return RedirectToAction("ShowCart", "Cart");
            }
        }

        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.RemoveItem(id);

            return RedirectToAction("ShowCart", "Cart");
        }

        public ActionResult Voucher()
        {
            var userId = Session["UserId"].ToString();
            var voucher = db.Vouchers.Where(m => m.AccountId.ToString() == userId).ToList();

            return PartialView("Voucher",voucher);
        }

        public ActionResult Address()
        {
            var userId = Session["UserId"].ToString();
            var userInfor = db.Accounts.Where(m => m.AccountId.ToString() == userId).FirstOrDefault();

            return PartialView("Address", userInfor);
        }

        public ActionResult Delivery()
        {
            var delivery = db.Deliveries.ToList();
            return PartialView("Delivery",delivery);
        }

        public ActionResult ApplyDelivery(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;

            var deliveryId = int.Parse(form["delivery-id"]);
            var delivery = db.Deliveries.Where(m => m.DeliveryId == deliveryId).FirstOrDefault();

            var finalPrice = cart.TotalMoney() + Decimal.ToDouble(delivery.DeliveryPrice.GetValueOrDefault());

            TempData["DiscountPrice"] = finalPrice;
            return RedirectToAction("ShowCart", "Cart");
        }

        public PartialViewResult BagCart()
        {
            int totalItem = 0;
            double totalMoney = 0;
            Cart cart = Session["Cart"] as Cart;

            if (cart != null)
            {
                totalItem = cart.TotalQuantity();
                totalMoney = cart.TotalMoney();
                ViewBag.QuantityCart = totalItem;
                ViewBag.TotalMoney = totalMoney;
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


        public ActionResult Purchase(FormCollection form)
        {
            try
            {
                var userName = Session["UserNameCustomer"];
                var userId = Session["UserId"].ToString();
                var userInfor = db.Accounts.FirstOrDefault(x => x.AccountId.ToString() == userId);
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