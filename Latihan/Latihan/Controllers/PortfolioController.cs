using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Latihan.Controllers
{
    public class PortfolioController : Controller
    {
        //
        // GET: /Portfolio/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Portfolio()
        {
            return View();
        }

    }
}
