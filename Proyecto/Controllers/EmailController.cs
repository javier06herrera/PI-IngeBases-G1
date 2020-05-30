using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class EmailController : Controller
    {
        // GET: Emails
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Emails()
        {
            return View();
        }

        public ActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(EmailModel emodel)
        {
            string senderMail = "ingebasesgrupo1@gmail.com";
            string password = "grupo12345";
            string subject = emodel.subject;
            string receiverMail = emodel.mail;        
            string messageContent = emodel.message;

            MailMessage mail = new MailMessage(senderMail, receiverMail, subject, messageContent);

            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com";

            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(senderMail, password);

            client.Send(mail);
            client.Dispose();

            return RedirectToAction("SendMail");
        }
    }

}