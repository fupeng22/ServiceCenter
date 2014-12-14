using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using System.Data;
using ServiceCenter.Filter;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class MapPortViewController : Controller
    {
        //
        // GET: /MapPortView/

         [LoginValidate]
        public ActionResult Index()
        {
            DataSet ds = null;
            DataTable dt = null;


            string MapId_Org = Request.QueryString["MapId"];
            string EquipmentId = Request.QueryString["EquipmentId"];
            string MapPotId = "";
            string MapId = "";

            if (!string.IsNullOrEmpty(EquipmentId))
            {
                ds = new T_MapPots().GetMapIdByEquipmentId(Convert.ToInt32(EquipmentId));
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        MapId_Org = dt.Rows[0]["MapId"].ToString();
                        MapPotId = dt.Rows[0]["Id"].ToString();
                        ViewData["hid_MapPotId"] = MapPotId;
                    }
                }
            }

            MapId = MapId_Org;
            if (string.IsNullOrEmpty(MapId))
            {
                //RegisterStartupScript("key", "<script type='text/javascript'>alert('输入参数不正确');</script>");
            }
            else
            {
                ds = null;
                dt = null;
                ds = new T_MapHeader().LoadMapInfoByMapId(Convert.ToInt32(MapId));
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ViewData["hid_MapId"] = dt.Rows[0]["Id"].ToString();
                        ViewData["hid_MapName"] = dt.Rows[0]["MapName"].ToString();
                        ViewData["hid_MapPath"] = dt.Rows[0]["MapPath"].ToString();
                        ViewData["hid_GroupName"] = dt.Rows[0]["GroupName"].ToString();
                    }
                }

            }



            return View();
        }
    }
}
