using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Util;
using DBUtility;
using System.Text;
using ServiceCenter.Models;
using SQLDAL;
using ServiceCenter.Filter;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class Query_StatisticByDayController : Controller
    {
        //
        // GET: /Query_StatisticByDay/
         [LoginValidate]
        public ActionResult Index()
        {
            string MapPotId_Org = Request.QueryString["MapPotId"];
            DataSet ds = null;
            DataTable dt = null;
            ds = new T_MapPots().GetMapPotInfo(Convert.ToInt32(MapPotId_Org));
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    ViewData["EquipmentId"] = dt.Rows[0]["EquipmentId"].ToString();
                    ViewData["DeviceName"] = dt.Rows[0]["DeviceName"].ToString();
                }
            }
            return View();
        }

        [HttpPost]
        public string GetData(string dYMD_Begin, string dYMD_End, string Station, string EmpNO)
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@YMD_Begin";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = Server.UrlDecode(dYMD_Begin);

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@YMD_End";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = Server.UrlDecode(dYMD_End);

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@Station";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = Server.UrlDecode(Station);

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.NVarChar;
            param[3].ParameterName = "@GroupIds";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = (Session["Global_UserName"] != null ? (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()) ? "" : (new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]))) : (new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"])));
            //new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"])

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.NVarChar;
            param[4].ParameterName = "@EmpNO";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Server.UrlDecode(EmpNO);

            DataSet ds = SqlServerHelper.RunProcedure("sp_WayBill_StatisticByDay", param, "result");
            DataTable dt = ds.Tables["result"];

            StringBuilder sb = new StringBuilder("");
            StringBuilder sb_wbNumber_Total = new StringBuilder("");
            StringBuilder sb_wbActualWeight_Total = new StringBuilder("");
            StringBuilder sb_wbVolume_Total = new StringBuilder("");

            sb_wbNumber_Total.Append("{name:'" + LangHelper.GetLangbyKey("StatisticByDay_WayBill_Control_Label1") + "',data:[");
            sb_wbActualWeight_Total.Append("{name:'" + LangHelper.GetLangbyKey("StatisticByDay_WayBill_Control_Label2") + "',data:[");
            sb_wbVolume_Total.Append("{name:'" + LangHelper.GetLangbyKey("StatisticByDay_WayBill_Control_Label3") + "',data:[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb_wbNumber_Total.AppendFormat("{0}", dt.Rows[i]["wbNumber_Total"].ToString());
                sb_wbActualWeight_Total.AppendFormat("{0}", dt.Rows[i]["wbActualWeight_Total"].ToString());
                sb_wbVolume_Total.AppendFormat("{0}", dt.Rows[i]["wbVolume_Total"].ToString());
                if (i != dt.Rows.Count - 1)
                {
                    sb_wbNumber_Total.Append(",");
                    sb_wbActualWeight_Total.Append(",");
                    sb_wbVolume_Total.Append(",");
                }

            }
            dt = null;
            sb_wbNumber_Total.Append("]},");
            sb_wbActualWeight_Total.Append("]},");
            sb_wbVolume_Total.Append("]}");

            sb.Append("[");
            sb.Append(sb_wbNumber_Total.ToString());
            sb.Append(sb_wbActualWeight_Total.ToString());
            sb.Append(sb_wbVolume_Total.ToString());
            sb.Append("]");
            return sb.ToString();
        }

    }
}
