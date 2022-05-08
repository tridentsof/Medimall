using Medimall.Infrastructure;
using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Medimall.Controllers
{
    [CustomAuthenticationCustomerFilter]
    public class AccountController : Controller
    {
        // GET: Account
        private MedimallEntities db = new MedimallEntities();
        public ActionResult Account()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var user = db.Accounts.Where(p => p.AccountId == accountId).FirstOrDefault();
            var pointToVIP = 2000000 - user.PowerPoint.GetValueOrDefault();
            decimal useablePoint = 0;
            TempData["PointToVIP"] = String.Format("{0:0,0}",pointToVIP);

            if(pointToVIP < 0)
            {
                useablePoint = Math.Abs(pointToVIP);
                TempData["UseablePoint"] = String.Format("{0:0,0}", useablePoint);
            }
            else
            {
                useablePoint = 0;
                TempData["UseablePoint"] = String.Format("{0:0,0}", useablePoint);
            }
            
            return PartialView(user);
        }
        public ActionResult GetAllOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).OrderByDescending(m => m.PurchaseDate).ToList();
            Billing status = db.Billings.Find(accountId);

            var checkVip = db.Accounts.Any(p => p.AccountId == accountId && p.IsVIP == true);

            if (checkVip)
            {
                ViewBag.CheckVip = "Xem chi tiết để biết điểm tích lũy";
                ViewBag.CSSClass = "point2";
            }    
            else
            {
                ViewBag.CheckVip = "Bạn cần lên vip để tích điểm";
                ViewBag.CSSClass = "point";
            }    
            return PartialView(displayoder);
        }

        public ActionResult GetWaitOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).OrderByDescending(m => m.PurchaseDate).Where(p => p.Status == 1).ToList();
            var checkVip = db.Accounts.Any(p => p.AccountId == accountId && p.IsVIP == true);

            if (checkVip)
            {
                ViewBag.CheckVip = "Xem chi tiết để biết điểm tích lũy";
                ViewBag.CSSClass = "point2";
            }
            else
            {
                ViewBag.CheckVip = "Bạn cần lên vip để tích điểm";
                ViewBag.CSSClass = "point";
            }
            return PartialView(displayoder);
        }

        public ActionResult GetDeliOder()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p => p.Status == 2).ToList();
            var checkVip = db.Accounts.Any(p => p.AccountId == accountId && p.IsVIP == true);

            if (checkVip)
            {
                ViewBag.CheckVip = "Xem chi tiết để biết điểm tích lũy";
                ViewBag.CSSClass = "point2";
            }
            else
            {
                ViewBag.CheckVip = "Bạn cần lên vip để tích điểm";
                ViewBag.CSSClass = "point";
            }
            return PartialView(displayoder);
        }

        public ActionResult GetSuccesOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p => p.Status == 3).ToList();
            var checkVip = db.Accounts.Any(p => p.AccountId == accountId && p.IsVIP == true);

            if (checkVip)
            {
                ViewBag.CheckVip = "Xem chi tiết để biết điểm tích lũy";
                ViewBag.CSSClass = "point2";
            }
            else
            {
                ViewBag.CheckVip = "Bạn cần lên vip để tích điểm";
                ViewBag.CSSClass = "point";
            }
            return PartialView(displayoder);
        }
        public ActionResult GetCancleOders()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayoder = db.Billings.Where(p => p.AccountId == accountId).Where(p => p.Status == 0).ToList();
            var checkVip = db.Accounts.Any(p => p.AccountId == accountId && p.IsVIP == true);

            if (checkVip)
            {
                ViewBag.CheckVip = "Xem chi tiết để biết điểm tích lũy";
                ViewBag.CSSClass = "point2";
            }
            else
            {
                ViewBag.CheckVip = "Bạn cần lên vip để tích điểm";
                ViewBag.CSSClass = "point";
            }
            return PartialView(displayoder);
        }
        public ActionResult GetAddressOder()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayaddress = db.Billings.Where(p => p.AccountId == accountId).ToList();
            return PartialView(displayaddress);
        }
        public ActionResult Edit()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var displayuser = db.Accounts.Where(p => p.AccountId == accountId).FirstOrDefault();
            return PartialView("Edit", displayuser);
        }
        [HttpPost]
        public ActionResult UpdateCustomer(Account customer)
        {
            var accountId = int.Parse(Session["UserId"].ToString()); 
            Account account= (from c in db.Accounts
                              where c.AccountId == accountId
                              select c).FirstOrDefault();
            if (account != null)
            {
                account.FullName = customer.FullName;
                account.BirthDay = customer.BirthDay;
                account.Phone = customer.Phone;
                account.Email = customer.Email;
                account.Gender = customer.Gender;
                db.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        public ActionResult Voucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var voucher = db.Vouchers.Where(p => p.AccountId == accountId).ToList();
            return PartialView(voucher);
        }
        public ActionResult ForceVoucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            //int? bill = db.Billings.Where(u => u.AccountId == accountId).Select(u => u.BillId).FirstOrDefault();


            //int? voucherid = db.BillDetails.Where(u => u.BillId == bill).Select(u => u.VoucherId).FirstOrDefault();

            var voucher = db.Vouchers.Where(p => p.EndDate > DateTime.Now).ToList();
            return PartialView(voucher);
        }

        public ActionResult ExpireVoucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            //int? bill = db.Billings.Where(u => u.AccountId == accountId).Select(u => u.BillId).FirstOrDefault();


            //int? voucherid = db.BillDetails.Where(u => u.BillId == bill).Select(u => u.VoucherId).FirstOrDefault();

            var voucher = db.Vouchers.Where(p => p.EndDate < DateTime.Now).ToList();
            return PartialView(voucher);
        }
        public ActionResult UsedVoucher()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var getbillid = db.Billings.Where(u => u.AccountId == accountId).Select(u => u.BillId).ToList();
            var getbilldetail = db.BillDetails.ToList();
            List<BillDetail> getAllDetail = new List<BillDetail>();
            var getAllVocuher = db.Vouchers.ToList();
            List<Voucher> getVoucher = new List<Voucher>();
            foreach (var item in getbillid)
            {

                for (int i = 0; i < getbilldetail.Count; i++)
                {
                    if (item == getbilldetail[i].BillId)
                    {
                        getAllDetail.Add(getbilldetail[i]);
                    }
                }
            }
            foreach (var item in getAllDetail)
            {
                for (int i = 0; i < getAllVocuher.Count; i++)
                {
                    if (item.VoucherId == getAllVocuher[i].VoucherId)
                    {
                        getVoucher.Add(getAllVocuher[i]);
                    }
                }
            }
            var getDistinctVoucher = getVoucher.Distinct().ToList();
            return PartialView(getDistinctVoucher);
        }
        public ActionResult OderDetail()
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());
            var diplasydetail = db.BillDetails.Where(p => p.BillId == id).ToList();
            return PartialView(diplasydetail);
        }
        public ActionResult OderDetailHeader()
        {
            int id = int.Parse(this.RouteData.Values["id"].ToString());
            var diplasydetail = db.Billings.Where(p => p.BillId == id).FirstOrDefault();
            return PartialView(diplasydetail);
        }
        public ActionResult OderDetailTotal()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            int  id = int.Parse(this.RouteData.Values["id"].ToString());

            List<BillDetail> billDetails = db.BillDetails.Where(u => u.BillId == id).ToList();

            decimal? Total = 0;

            foreach(var item in billDetails)
            {
                for (int i = 0; i < billDetails.Count; i++)
                {
                    Total = Total+(item.Price * item.Quantity);
                    i++;
                }
            }    
             int ? getiddeli = db.Billings.Where(u => u.BillId == id).Select(u => u.DeliveryId).FirstOrDefault();

            decimal DeliPrice = (decimal)db.Deliveries.Where(p => p.DeliveryId == getiddeli).Select(p => p.DeliveryPrice).FirstOrDefault();
            decimal? PromotionPrice = db.Billings.Where(p => p.BillId == id).Select(p => p.PromotionPrice).FirstOrDefault();

            ViewBag.PromotionPrice = PromotionPrice;
            ViewBag.Total = Total;
            ViewBag.PriceDeli = DeliPrice;

            decimal ? SumTotal = Total + DeliPrice- PromotionPrice;
            ViewBag.Sumtotal = SumTotal;

            var displayDetail = db.BillDetails.Where(p => p.BillId == id).ToList();

            var checkVip = db.Accounts.Any(p => p.AccountId == accountId && p.IsVIP == true);

            var ListProduct = db.Products.ToList();

            decimal? sumEarnPoint = 0;

            if (checkVip)
            {

                foreach (var item in billDetails)
                {
                    foreach (var itemProduct in ListProduct)
                    {
                        if (item.ProductId == itemProduct.ProductId)
                        {
                            var product = db.Products.Where(p => p.ProductId == itemProduct.ProductId).FirstOrDefault();


                            var earnPoint = (product.PercentSalePoint / 100) * product.Price;
                            sumEarnPoint += earnPoint;
                        }
                    }
                }
                ViewBag.EarnPoint = sumEarnPoint;
            }    
            else
            {
                ViewBag.EarnPoint = "Đang cập nhật";
            }    


                return PartialView(displayDetail);
        }
        public JsonResult UpdatePassword(string oldPass,string newPass) 
        {
            db.Configuration.ProxyCreationEnabled = false;
            var accountId = int.Parse(Session["UserId"].ToString());
            var checkvalid = db.Accounts.Any(p => p.AccountId == accountId && p.Password == oldPass);
            Account account = (from c in db.Accounts
                               where c.AccountId == accountId
                               select c).FirstOrDefault();
            if(checkvalid)
            {
                if (account != null)
                {
                    account.Password = newPass;
                    db.SaveChanges();
                    return Json(true);
                }
            }   
            return Json(false);
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
        public ActionResult HealthDeclaration()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var healthBook = db.HealthBooks.Where(p => p.AccountId == accountId).FirstOrDefault();
            
            if(healthBook.IsValid == 0)
            {
                ViewBag.Valid = "Chờ xác thực";
            }    
            else if(healthBook.IsValid==2)
            {
                ViewBag.Valid = "Khai báo về mũi tiêm của bạn không chính xác,mời bạn khai báo lại";
            }    



            return PartialView(healthBook);
        } 

        public JsonResult UpdateHealthBook(HealthBook data)
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            HealthBook healthBook = new HealthBook();
            HealthBook healthBook2 = (from c in db.HealthBooks
                                     where c.AccountId == accountId
                                     select c).FirstOrDefault();
            Account account = (from c in db.Accounts
                               where c.AccountId == accountId
                               select c).FirstOrDefault();
            if(healthBook2 == null)
            {
                account.IsHealthCare = true;
                healthBook.AccountId = accountId;
                healthBook.IsVaccinated = data.IsVaccinated;
                healthBook.IsCovid = data.IsCovid;
                healthBook.DoseOne = data.DoseOne;
                healthBook.DoseTwo = data.DoseTwo;
                healthBook.DoseThree = data.DoseThree;
                healthBook.MedicalHistory = data.MedicalHistory;
                healthBook.Symptoms = data.Symptoms;
                db.HealthBooks.Add(healthBook);
                db.SaveChanges();
                return Json(true);
            }    
            else
            {
                if(data.Symptoms != null)
                {
                    healthBook2.IsVaccinated = data.IsVaccinated;
                    healthBook2.IsCovid = data.IsCovid;
                    healthBook2.DoseOne = data.DoseOne;
                    healthBook2.DoseTwo = data.DoseTwo;
                    healthBook2.DoseThree = data.DoseThree;
                    healthBook2.MedicalHistory = data.MedicalHistory;
                    healthBook2.Symptoms = data.Symptoms;
                }
                else 
                {
                    healthBook2.IsVaccinated = data.IsVaccinated;
                    healthBook2.IsCovid = data.IsCovid;
                    healthBook2.DoseOne = data.DoseOne;
                    healthBook2.DoseTwo = data.DoseTwo;
                    healthBook2.DoseThree = data.DoseThree;
                    healthBook2.MedicalHistory = data.MedicalHistory;
                }

                db.SaveChanges();
                return Json(true);
            }
        }

        public ActionResult HealthBook()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var healthBook = db.HealthBooks.Where(p => p.AccountId == accountId && p.IsValid==1).FirstOrDefault();
            var accountCheck = db.Accounts.Where(p => p.AccountId == accountId).FirstOrDefault();

            var getCovid = accountCheck.IsHealthCare;

            if (getCovid == true)
            {
                ViewBag.Covid = "Đã mắc covid";
            }
            else ViewBag.Covid = "Chưa mắc covid";

            if(healthBook != null)
            {
                var getSymptoms = healthBook.Symptoms;

                if (getSymptoms == null)
                {

                    ViewBag.Symptoms = "Không có triệu chứng";
                }
            }    

            return PartialView(healthBook);
        }
        public ActionResult RankRule()
        {
            var accountId = int.Parse(Session["UserId"].ToString());
            var user = db.Accounts.Where(p => p.AccountId == accountId).FirstOrDefault();
            return PartialView(user);
        }
    }
}