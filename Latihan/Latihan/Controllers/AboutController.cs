using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Latihan.Models;
using System.Net;
using System.Net.Mail; 
using System.Web.Helpers;
namespace Latihan.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            List<Person> person = new List<Person>
            {
                new Person { Name = "Iqbal", Kota = "Depok" },
                new Person { Name = "Brandon", Kota = "Bogor" },
                new Person { Name = "Julian", Kota = "Tangerang" }
            };
            return View(person);
        }

        public ActionResult ProcessRequest() 
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            //Or your Smtp Email ID and Password
            smtp.EnableSsl = true;
            return View();
        }
        //send mail

        public ActionResult Action(string name, string email, string subject)
        {
            //ViewData["name"] = name;
            //ViewData["email"] = email;
            //ViewData["subject"] = subject;
            ViewData["status"] = "a";
            string customerName = name;
            ViewData["customerName"] = customerName;

            string customerEmail = email;

            string customerRequest = subject;
            ViewData["customerRequest"] = customerRequest;

            string errorMessage = "";
            ViewData["errorMessage"] = errorMessage;

            string debuggingFlag = null;
            ViewData["debuggingFlag"] = debuggingFlag;

            if (customerEmail == null && customerName == null && customerRequest == null)
            {
                ViewData["status"] = "a";
            }
            else
            {
                try
                {
                    ViewData["status"] = "b";
                    // Initialize WebMail helper
                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.SmtpPort = 587;
                    WebMail.UserName = "heryanakasih@gmail.com";
                    WebMail.Password = "teizluhdljeuzorf";
                    WebMail.From = "heryanakasih@gmail.com";
                    WebMail.EnableSsl = true;
                    // Send email
                    WebMail.Send(to: "heryanakasih@gmail.com",
                    subject: "Help request from - " + customerName + " [Email Address: " + customerEmail + "]",
                    body: customerRequest
                    );
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }

            return View("Index");
        }
    }
}
