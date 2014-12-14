using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceCenter.Filter;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using SQLDAL;
using Util;
using ServiceCenter.Models;

namespace ServiceCenter.Controllers
{
     [ErrorAttribute]
    public class StatisticFeeByMonthController : Controller
    {
        //
        // GET: /StatisticByDay_WayBill/
         [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
         public string GetData(string dYM_Begin, string dYM_End, string Station, string EmpNO)
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@YM_Begin";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = Server.UrlDecode(dYM_Begin);

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@YM_End";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = Server.UrlDecode(dYM_End);

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

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.NVarChar;
            param[4].ParameterName = "@EmpNO";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Server.UrlDecode(EmpNO);

            DataSet ds = SqlServerHelper.RunProcedure("sp_StatisticFeeByMonth", param, "result");
            DataTable dt = ds.Tables["result"];

            StringBuilder sb = new StringBuilder("");
            StringBuilder sb_wbNumber_Weight_Total = new StringBuilder("");
            StringBuilder sb_wbNumber_Volume_Total = new StringBuilder("");

            sb_wbNumber_Weight_Total.Append("{name:'" + LangHelper.GetLangbyKey("StatisticFeeByMonth_Control_Tip1") + "',data:[");
            sb_wbNumber_Volume_Total.Append("{name:'" + LangHelper.GetLangbyKey("StatisticFeeByMonth_Control_Tip2") + "',data:[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb_wbNumber_Weight_Total.AppendFormat("{0}", dt.Rows[i]["wbNumber_Total_ByWeight"].ToString());
                sb_wbNumber_Volume_Total.AppendFormat("{0}", dt.Rows[i]["wbNumber_Total_ByVolume"].ToString());
                
                if (i != dt.Rows.Count - 1)
                {
                    sb_wbNumber_Weight_Total.Append(",");
                    sb_wbNumber_Volume_Total.Append(",");
                }

            }
            dt = null;
            sb_wbNumber_Weight_Total.Append("]},");
            sb_wbNumber_Volume_Total.Append("]},");
           
            sb.Append("[");
            sb.Append(sb_wbNumber_Weight_Total.ToString());
            sb.Append(sb_wbNumber_Volume_Total.ToString());
            sb.Append("]");
            return sb.ToString();
        }

    }
}
