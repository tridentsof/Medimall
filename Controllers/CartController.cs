using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medimall.Infrastructure;

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

            if(quantityLeft >= quantity)
            {
                TempData["SuccessQuantity"] = "Cập nhật số lượng thành công";
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

        [HttpPost]
        [CustomAuthenticationCustomerFilter]
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
                Session["DiscountPrice"] = discountPrice;
                TempData["SuccessVoucher"] = "Áp dụng thành công!";
                Session["VoucherId"] = idVoucher;
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
            var voucher = db.Vouchers.ToList();

            return PartialView("Voucher",voucher);
        }

        public ActionResult Address()
        {
            string userId = String.Empty;
            if (Session["UserId"] == null)
            {
                userId = "0";
            }
            else
            {
                userId = Session["UserId"].ToString();
            }

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

            var deliveryPrice = delivery.DeliveryPrice; 

            Session["DeliveryPrice"] = deliveryPrice;
            Session["DeliveryId"] = deliveryId;
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


        [HttpPost]
        [CustomAuthenticationCustomerFilter]
        public ActionResult Purchase(FormCollection form)
        {
            try
            {
                var userName = Session["UserNameCustomer"];
                var userId = Session["UserId"].ToString();
                var userInfor = db.Accounts.FirstOrDefault(x => x.AccountId.ToString() == userId);
                var voucherId = Convert.ToInt32(Session["VoucherId"]);
                var voucher = db.Vouchers.FirstOrDefault(m => m.VoucherId == voucherId);
                var deliveryId = Convert.ToInt32(Session["DeliveryId"]);
                var delivery = db.Deliveries.FirstOrDefault(m => m.DeliveryId == deliveryId);

                Cart cart = Session["Cart"] as Cart;
                Billing billing = new Billing();

                billing.AccountId = userInfor.AccountId;
                billing.PurchaseDate = DateTime.Now;
                //Can replace if adress in form collection == null
                billing.Address = userInfor.Address;
                billing.PayId = int.Parse(form["pay-id"]);
                billing.DeliveryId = deliveryId;
                billing.Total = int.Parse(form["total-money"]);
                billing.Phone = userInfor.Phone;
                billing.UserName = userInfor.UserName;

                billing.Status = 1;
                db.Billings.Add(billing);

                foreach (var item in cart.Items)
                {
                    BillDetail billDetail = new BillDetail();

                    var product = db.Products.Single(x => x.ProductId == item._shopping_product.ProductId);

                    product.QuantitySold += item._shopping_quantity;
                    product.Quantity -= item._shopping_quantity;

                    var totalMoney = Convert.ToDecimal(item._shopping_product.Price * (100 - voucher.Percent.GetValueOrDefault()) / 100 + delivery.DeliveryPrice.GetValueOrDefault());

                    billDetail.BillId = billing.BillId;
                    billDetail.ProductId = item._shopping_product.ProductId;
                    billDetail.Price = item._shopping_product.Price;
                    billDetail.Quantity = item._shopping_quantity;
                    billDetail.ProductName = item._shopping_product.ProductName;
                    billDetail.VoucherId = voucherId;
                    billDetail.Total = totalMoney;

                    db.BillDetails.Add(billDetail);
                }

                db.SaveChanges();
                TempData["SuccessCart"] = "Thanh toán thành công, mời bạn vào lịch sử đơn hàng để theo dõi";
                cart.ClearCart();
                Session["DiscountPrice"] = null;
                Session["VoucherId"] = null;
                Session["DeliveryPrice"] = null;
                Session["DeliveryId"] = null;

                return RedirectToAction("Index", "Home");

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}