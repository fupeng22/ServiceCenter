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
    public class StatisticFeeByMonth_PieController : Controller
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

            if (dt!=null && dt.Rows.Count>0)
            {
                sb.Append("[{");
                sb.Append("type: 'pie',");
                sb.Append("name: '" + LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip1") + "',");
                sb.Append("data: [");
                if (Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByWeight"].ToString()) + Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByVolume"].ToString()) == 0)
                {
                    sb.AppendFormat("['{0}', {1}],", string.Format(LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip2") + "(" + LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip4") + ":{0})", Convert.ToInt32(dt.Rows[0]["wbNumber_Total_ByWeight"].ToString())), 0.00);
                    sb.AppendFormat("['{0}', {1}]", string.Format(LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip3") + "(" + LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip4") + ":{0})", Convert.ToInt32(dt.Rows[0]["wbNumber_Total_ByVolume"].ToString())), 0.00);
                }
                else
                {
                    sb.AppendFormat("['{0}', {1}],", string.Format(LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip2") + "(" + LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip4") + ":{0})", Convert.ToInt32(dt.Rows[0]["wbNumber_Total_ByWeight"].ToString())), (Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByWeight"].ToString()) / (Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByWeight"].ToString()) + Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByVolume"].ToString()))).ToString("0.00"));
                    sb.AppendFormat("['{0}', {1}]", string.Format(LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip3") + "(" + LangHelper.GetLangbyKey("StatisticFeeByMonth_Pie_Control_Tip4") + ":{0})", Convert.ToInt32(dt.Rows[0]["wbNumber_Total_ByVolume"].ToString())), (Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByVolume"].ToString()) / (Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByWeight"].ToString()) + Convert.ToDouble(dt.Rows[0]["wbNumber_Total_ByVolume"].ToString()))).ToString("0.00"));
                }
                
                sb.Append("]");
                sb.Append("}]");
            }
           
            return sb.ToString();
        }

    }
}
