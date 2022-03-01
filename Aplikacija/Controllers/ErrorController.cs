using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aplikacija.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult BadRequest()
        {
            return View();
        }

        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}