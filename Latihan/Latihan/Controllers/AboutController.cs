using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Latihan.Models;
using System.Net;
using System.Net.Mail; 

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
    }
}
