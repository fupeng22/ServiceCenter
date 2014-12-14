using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using SQLDAL;
using ServiceCenter.Filter;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class MapRealController : Controller
    {
        //
        // GET: /MapReal/

         [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string LoadDefaultSeleMap()
        {
            string strRet = "";
            DataSet ds = null;
            DataTable dt = null;
            ds = new T_BaseDictionary().LoadDefaultSeleMap("DefaultSeleMap", "Admin");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0]["sValue"].ToString();
                }
            }

            return strRet;
        }

        [HttpGet]
        public string AddDefaultSeleMap(string defaultMapSele)
        {
            string strRet = "";
            string tmp_defaultMapSele = Server.UrlDecode(defaultMapSele);
            new T_BaseDictionary().AddDefaultSeleMap("DefaultSeleMap", "Admin", tmp_defaultMapSele);
            return strRet;
        }
    }
}
