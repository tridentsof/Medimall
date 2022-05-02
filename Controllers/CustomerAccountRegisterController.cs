using Medimall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Medimall.Controllers
{
    public class CustomerAccountRegisterController : Controller
    {
        // GET: CustomerAccountRegister
        private MedimallEntities db = new MedimallEntities();
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateAccount(Account customer)
        {
            var checkuser = db.Accounts.Any(p => p.UserName == customer.UserName);
            Account account = (from c in db.Accounts
                               select c).FirstOrDefault();
            bool result = checkuser ? false : true;
            if (checkuser)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
                db.Accounts.Add(customer);
                db.SaveChanges();
                BuildEmailTemplate(customer.AccountId);
                return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirm(int accountId)
        {
            ViewBag.ID = accountId;

            return View();
        }
        public JsonResult RegisterConfirm(int accountId)
        {
            Account account = db.Accounts.Where(x => x.AccountId == accountId).FirstOrDefault();
            account.Status = 1;
            db.SaveChanges();
            TempData["SuccessEmail"] = "Tài khoản của bạn đã được xác nhận";
            var msg = "Email đã được xác thực";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public void BuildEmailTemplate(int accountId)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var accountinfo = db.Accounts.Where(x => x.AccountId == accountId).FirstOrDefault();
            var url = "https://localhost:44394/" + "CustomerAccountRegister/Confirm?accountid=" + accountId;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Tài khoản của bạn cần được xác thực", body, accountinfo.Email);
        }
        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "proxgroupcdio4@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);

        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("proxgroupcdio4@gmail.com", "ProxTichHop2022");
            try
            {
                client.Send(mail);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}