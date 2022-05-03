using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medimall.Infrastructure;
using System.Configuration;
using Medimall.Helper;

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

            var userId = Session["UserId"]?.ToString() ?? "0";
            var userInfor = db.Accounts.Where(m => m.AccountId.ToString() == userId).FirstOrDefault();

            if(userInfor != null)
            {
                var isVipCustomer = db.Accounts.Any(n => n.AccountId == userInfor.AccountId && n.IsVIP == true);
                ViewBag.IsVipCustomer = isVipCustomer;

                var pointToVIP = 2000000 - userInfor.PowerPoint.GetValueOrDefault();
                decimal useablePoint = 0;

                if (pointToVIP < 0)
                {
                    useablePoint = Math.Abs(pointToVIP);
                    TempData["PowerPoint"] = useablePoint;
                }
                else
                {
                    useablePoint = 0;
                    TempData["PowerPoint"] = useablePoint;
                }
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

            if (quantityLeft >= quantity)
            {
                TempData["SuccessQuantity"] = "Cập nhật số lượng thành công";
                cart.UpdateQuantity(productId, quantity);
                return RedirectToAction("ShowCart", "Cart");
            }
            else
            {
                var alert = String.Format("Xin lỗi quý khách! số lượng sản phẩm còn lại trong kho: {0}", quantityLeft);
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

            if (cart.TotalMoney() < Decimal.ToDouble(startFrom))
            {
                TempData["ErrorVoucher"] = "Giá trị đơn hàng chưa đủ để áp dụng khuyến mãi!";
                return RedirectToAction("ShowCart", "Cart");
            }
            else
            {
                double discountPrice = cart.TotalMoney() * (100 - percentDiscount) / 100;
                Session["DiscountPrice"] = discountPrice;
                Session["PriceSale"] = cart.TotalMoney() - discountPrice;
                TempData["SuccessVoucher"] = "Áp dụng thành công!";
                Session["VoucherId"] = idVoucher;
                return RedirectToAction("ShowCart", "Cart");
            }
        }

        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.RemoveItem(id);
            Session["DeliveryPrice"] = null;
            Session["DeliveryId"] = null;
            Session["VoucherId"] = null;
            Session["DiscountPrice"] = null;
            Session["PriceSale"] = null;

            return RedirectToAction("ShowCart", "Cart");
        }

        public ActionResult Voucher()
        {
            var voucher = db.Vouchers.Where(m => m.EndDate > DateTime.Now).ToList();

            return PartialView("Voucher", voucher);
        }

        public ActionResult Address()
        {
            var userId = Session["UserId"]?.ToString() ?? "0";
            var userInfor = db.Accounts.Where(m => m.AccountId.ToString() == userId).FirstOrDefault();

            return PartialView("Address", userInfor);
        }

        public ActionResult Delivery()
        {
            var delivery = db.Deliveries.ToList();
            return PartialView("Delivery", delivery);
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
            Cart cart = Session["Cart"] as Cart;

            if (cart != null)
            {
                int totalItem = cart.TotalQuantity();
                double totalMoney = cart.TotalMoney();
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
                var deliveryId = Convert.ToInt32(Session["DeliveryId"]);
                var paymentMethod = int.Parse(form["pay-id"]);
                var userName = form["user-name"];
                var phoneNumber = form["user-phone"];
                var address = form["user-address"];
                var userId = Session["UserId"].ToString();
                var userInfor = db.Accounts.FirstOrDefault(x => x.AccountId.ToString() == userId);
                var voucherId = Convert.ToInt32(Session["VoucherId"]);
                var voucher = db.Vouchers.FirstOrDefault(m => m.VoucherId == voucherId);
                var delivery = db.Deliveries.FirstOrDefault(m => m.DeliveryId == deliveryId);
                var isUsePoint = int.Parse(form["is-use-point"]);
                var pointUsed = 0; 
                if (isUsePoint == Constants.IsUsePoint.Use)
                {
                    pointUsed = int.Parse(form["point-used"]); ;
                    userInfor.PowerPoint -= Convert.ToDecimal(pointUsed);
                    userInfor.UsedPoint += pointUsed;
                }
                var salePrice = Convert.ToDecimal(form["sales-price"]);
                

                if (paymentMethod == Constants.PaymentMethod.PayLater)
                {
                    //Normal Payment
                    Cart cart = Session["Cart"] as Cart;

                    Billing billing = new Billing();
                    billing.AccountId = userInfor.AccountId;
                    billing.PurchaseDate = DateTime.Now;
                    billing.Address = address;
                    billing.PayId = int.Parse(form["pay-id"]);
                    billing.DeliveryId = deliveryId;
                    billing.Total = int.Parse(form["total-money"]);
                    billing.Phone = phoneNumber;
                    billing.UserName = userName;
                    billing.Status = 1;
                    billing.PromotionPrice = salePrice;

                    db.Billings.Add(billing);

                    foreach (var item in cart.Items)
                    {
                        BillDetail billDetail = new BillDetail();

                        var product = db.Products.Single(x => x.ProductId == item._shopping_product.ProductId);
                        var earnPoint = item._shopping_product.Price * item._shopping_product.PercentSalePoint / 100;

                        product.QuantitySold += item._shopping_quantity;
                        product.Quantity -= item._shopping_quantity;

                        billDetail.BillId = billing.BillId;
                        billDetail.ProductId = item._shopping_product.ProductId;
                        billDetail.Price = item._shopping_product.Price;
                        billDetail.Quantity = item._shopping_quantity;
                        billDetail.ProductName = item._shopping_product.ProductName;
                        billDetail.Total = item._shopping_product.Price * item._shopping_quantity;
                        userInfor.PowerPoint += earnPoint;

                        db.BillDetails.Add(billDetail);
                    }

                    db.SaveChanges();
                    TempData["SuccessCart"] = "Đặt hàng thành công, mời bạn vào lịch sử đơn hàng để theo dõi";
                    cart.ClearCart();
                    Session["DiscountPrice"] = null;
                    Session["VoucherId"] = null;
                    Session["DeliveryPrice"] = null;
                    Session["DeliveryId"] = null;

                    return RedirectToAction("Index", "Home");
                }
                else if(paymentMethod == Constants.PaymentMethod.Online)
                {
                    //Online Payment
                    string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
                    string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
                    string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
                    string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat

                    Cart cart = Session["Cart"] as Cart;

                    Billing billing = new Billing();
                    billing.AccountId = userInfor.AccountId;
                    billing.PurchaseDate = DateTime.Now;
                    billing.Address = address;
                    billing.PayId = int.Parse(form["pay-id"]);
                    billing.DeliveryId = deliveryId;
                    billing.Total = int.Parse(form["total-money"]);
                    billing.Phone = phoneNumber;
                    billing.UserName = userName;
                    billing.Status = 1;
                    billing.PromotionPrice = salePrice;

                    db.Billings.Add(billing);

                    foreach (var item in cart.Items)
                    {
                        BillDetail billDetail = new BillDetail();

                        var product = db.Products.Single(x => x.ProductId == item._shopping_product.ProductId);
                        var earnPoint = item._shopping_product.Price * item._shopping_product.PercentSalePoint / 100;

                        product.QuantitySold += item._shopping_quantity;
                        product.Quantity -= item._shopping_quantity;

                        billDetail.BillId = billing.BillId;
                        billDetail.ProductId = item._shopping_product.ProductId;
                        billDetail.Price = item._shopping_product.Price;
                        billDetail.Quantity = item._shopping_quantity;
                        billDetail.ProductName = item._shopping_product.ProductName;
                        billDetail.Total = item._shopping_product.Price * item._shopping_quantity;
                        userInfor.PowerPoint += earnPoint;

                        db.BillDetails.Add(billDetail);
                    }

                    db.SaveChanges();

                    //Build URL for VNPay
                    string locale = "vn";
                    VnPayLibrary vnpay = new VnPayLibrary();
                    vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                    vnpay.AddRequestData("vnp_Command", "pay");
                    vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                    vnpay.AddRequestData("vnp_Amount", ((int.Parse(form["total-money"])) * 100).ToString());
                    vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    vnpay.AddRequestData("vnp_CurrCode", "VND");
                    vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
                    if (!string.IsNullOrEmpty(locale))
                    {
                        vnpay.AddRequestData("vnp_Locale", locale);
                    }
                    else
                    {
                        vnpay.AddRequestData("vnp_Locale", "vn");
                    }
                    vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang tu Medimall");
                    vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
                    vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                    vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
                    //Add Params of 2.1.0 Version
                    vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddDays(1).ToString("yyyyMMddHHmmss"));
                    //Billing
                    vnpay.AddRequestData("vnp_Bill_Mobile", userInfor.Phone.ToString());
                    vnpay.AddRequestData("vnp_Bill_Email", userInfor.Email.ToString());
                    var fullName = userInfor.FullName;
                    if (!String.IsNullOrEmpty(fullName))
                    {
                        var indexof = fullName.IndexOf(' ');
                        vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                        vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
                    }
                    vnpay.AddRequestData("vnp_Bill_Address", userInfor.Address.Trim().ToString());
                    vnpay.AddRequestData("vnp_Bill_City", "Tam Ky");
                    vnpay.AddRequestData("vnp_Bill_Country", "Viet Nam");
                    vnpay.AddRequestData("vnp_Bill_State", "");
                    vnpay.AddRequestData("vnp_Inv_Phone", userInfor.Phone.Trim().ToString());
                    vnpay.AddRequestData("vnp_Inv_Email", userInfor.Email.Trim().ToString());
                    vnpay.AddRequestData("vnp_Inv_Customer", userInfor.UserName.Trim().ToString());
                    vnpay.AddRequestData("vnp_Inv_Address", userInfor.Address.Trim().ToString());
                    vnpay.AddRequestData("vnp_Inv_Company", "");
                    vnpay.AddRequestData("vnp_Inv_Taxcode", "0102182292");
                    vnpay.AddRequestData("vnp_Inv_Type", "I");


                    string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                    cart.ClearCart();
                    Session["DiscountPrice"] = null;
                    Session["VoucherId"] = null;
                    Session["DeliveryPrice"] = null;
                    Session["DeliveryId"] = null;
                    return Redirect(paymentUrl);
                }

                return RedirectToAction("Index", "Home");

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toán thành công
                        TempData["SuccessCart"] = "Đặt hàng thành công, mời bạn vào lịch sử đơn hàng để theo dõi";
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        TempData["FailCart"] = "Đặt hàng thất bại";
                    }
                }
                else
                {
                    TempData["FailCart"] = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}