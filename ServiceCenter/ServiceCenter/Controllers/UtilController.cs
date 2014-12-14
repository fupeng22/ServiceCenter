using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceCenter.Filter;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class UtilController : Controller
    {
        //
        // GET: /Util/
        [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string getDatetime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
