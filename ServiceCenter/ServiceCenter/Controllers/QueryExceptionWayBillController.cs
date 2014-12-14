using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using SQLDAL;
using ServiceCenter.Filter;
using Util;
using ServiceCenter.Models;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class QueryExceptionWayBillController : Controller
    {
        public const string strFileds = "wbName,wbNumber,wbActualWeight,wbVolume,wbScanTime,wbOperator,wbWeightByVolume,FeeFlag,FeeFlagDesc,ExceptionFlag,ExceptionFlagDesc,wbWidth,wbLength,wbHeight,mMemo,GroupID,GroupName,EmpNO,EmpName,wbId";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/QueryExceptionWayBill.rdlc";
        [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 分页查询类
        /// </summary>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public string GetData(string order, string page, string rows, string sort, string dBegin, string dEnd, string strOperator, string feeFlag, string exceptionFlag, string EmpNO)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_ScanLog";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "wbId";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;
            param[6].Value = "";

            string strWhereTemp = "";
            dBegin = Server.UrlDecode(dBegin);
            dEnd = Server.UrlDecode(dEnd);
            strOperator = Server.UrlDecode(strOperator);
            feeFlag = Server.UrlDecode(feeFlag);
            exceptionFlag = Server.UrlDecode(exceptionFlag);
            EmpNO = Server.UrlDecode(EmpNO);

            if (dBegin != "" && dEnd != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (convert(nvarchar(19),wbScanTime,120)>='{0}' and convert(nvarchar(19),wbScanTime,120)<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (convert(nvarchar(19),wbScanTime,120)>='{0}' and convert(nvarchar(19),wbScanTime,120)<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (strOperator != "-99" && strOperator != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (wbOperator='{0}') ", strOperator);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (wbOperator='{0}') ", strOperator);
                }
            }

            if (EmpNO != "-99" && EmpNO != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (EmpNO='{0}') ", EmpNO);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (EmpNO='{0}') ", EmpNO);
                }
            }

            if (feeFlag != "-99" && feeFlag != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (FeeFlag={0}) ", feeFlag);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (FeeFlag={0}) ", feeFlag);
                }
            }

            if (exceptionFlag != "-99" && exceptionFlag != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (ExceptionFlag={0}) ", exceptionFlag);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (ExceptionFlag={0}) ", exceptionFlag);
                }
            }
            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {

                }
                else
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (GroupID in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   (GroupID in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                }
            }
            else
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (GroupID in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (GroupID in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
            }
         
            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];

            StringBuilder sb = new StringBuilder("");
            sb.Append("{");
            sb.AppendFormat("\"total\":{0}", Convert.ToInt32(param[7].Value.ToString()));
            sb.Append(",\"rows\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("{");

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        default:
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", "")));
                            }
                            break;
                    }
                }

                if (i == dt.Rows.Count - 1)
                {
                    sb.Append("}");
                }
                else
                {
                    sb.Append("},");
                }
            }
            dt = null;
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }


        [HttpGet]
        public ActionResult Print(string order, string page, string rows, string sort, string dBegin, string dEnd, string strOperator, string feeFlag, string exceptionFlag, string EmpNO)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_ScanLog";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "wbId";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;
            param[6].Value = "";

            string strWhereTemp = "";
            dBegin = Server.UrlDecode(dBegin);
            dEnd = Server.UrlDecode(dEnd);
            strOperator = Server.UrlDecode(strOperator);
            feeFlag = Server.UrlDecode(feeFlag);
            exceptionFlag = Server.UrlDecode(exceptionFlag);
            EmpNO = Server.UrlDecode(EmpNO);

            if (dBegin != "" && dEnd != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (convert(nvarchar(19),wbScanTime,120)>='{0}' and convert(nvarchar(19),wbScanTime,120)<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (convert(nvarchar(19),wbScanTime,120)>='{0}' and convert(nvarchar(19),wbScanTime,120)<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (strOperator != "-99" && strOperator != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (wbOperator='{0}') ", strOperator);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (wbOperator='{0}') ", strOperator);
                }
            }

            if (EmpNO != "-99" && EmpNO != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (EmpNO='{0}') ", EmpNO);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (EmpNO='{0}') ", EmpNO);
                }
            }

            if (feeFlag != "-99" && feeFlag != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (FeeFlag={0}) ", feeFlag);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (FeeFlag={0}) ", feeFlag);
                }
            }

            if (exceptionFlag != "-99" && exceptionFlag != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (ExceptionFlag={0}) ", exceptionFlag);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (ExceptionFlag={0}) ", exceptionFlag);
                }
            }
            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {

                }
                else
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (GroupID in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   (GroupID in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                }
            }
            else
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (GroupID in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (GroupID in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
            }
            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            //wbName,wbNumber,wbActualWeight,wbVolume,wbScanTime,wbOperator,wbWeightByVolume,FeeFlag,
            //FeeFlagDesc,ExceptionFlag,ExceptionFlagDesc,wbWidth,wbLength,wbHeight,mMemo,wbId
            dtCustom.Columns.Add("wbName", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbActualWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbVolume", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbScanTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbOperator", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbWeightByVolume", Type.GetType("System.String"));
            dtCustom.Columns.Add("FeeFlag", Type.GetType("System.String"));
            dtCustom.Columns.Add("FeeFlagDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("ExceptionFlag", Type.GetType("System.String"));
            dtCustom.Columns.Add("ExceptionFlagDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbWidth", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbLength", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbHeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("mMemo", Type.GetType("System.String"));
            dtCustom.Columns.Add("GroupID", Type.GetType("System.String"));
            dtCustom.Columns.Add("GroupName", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpNO", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpName", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbId", Type.GetType("System.String"));

            DataRow drCustom = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }

                }
                if (drCustom["wbId"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("QueryExceptionWayBill_DS", dtCustom);

            localReport.DataSources.Add(reportDataSource);
            string reportType = "PDF";
            string mimeType;
            string encoding = "UTF-8";
            string fileNameExtension;

            string deviceInfo = "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>12in</PageWidth>" +
                " <PageHeigth>11in</PageHeigth>" +
                " <MarginTop>0.5in</MarginTop>" +
                " <MarginLeft>1in</MarginLeft>" +
                " <MarginRight>1in</MarginRight>" +
                " <MarginBottom>0.5in</MarginBottom>" +
                " </DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            return File(renderedBytes, mimeType);
        }


        [HttpGet]
        public ActionResult Excel(string order, string page, string rows, string sort, string dBegin, string dEnd, string strOperator, string feeFlag, string exceptionFlag, string EmpNO, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_ScanLog";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "wbId";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = sort + " " + order;

            param[4] = new SqlParameter();
            param[4].SqlDbType = SqlDbType.Int;
            param[4].ParameterName = "@PageSize";
            param[4].Direction = ParameterDirection.Input;
            param[4].Value = Convert.ToInt32(rows);

            param[5] = new SqlParameter();
            param[5].SqlDbType = SqlDbType.Int;
            param[5].ParameterName = "@PageCurrent";
            param[5].Direction = ParameterDirection.Input;
            param[5].Value = Convert.ToInt32(page);

            param[6] = new SqlParameter();
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].ParameterName = "@Where";
            param[6].Direction = ParameterDirection.Input;
            param[6].Value = "";

            string strWhereTemp = "";
            dBegin = Server.UrlDecode(dBegin);
            dEnd = Server.UrlDecode(dEnd);
            strOperator = Server.UrlDecode(strOperator);
            feeFlag = Server.UrlDecode(feeFlag);
            exceptionFlag = Server.UrlDecode(exceptionFlag);
            EmpNO = Server.UrlDecode(EmpNO);

            if (dBegin != "" && dEnd != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (convert(nvarchar(19),wbScanTime,120)>='{0}' and convert(nvarchar(19),wbScanTime,120)<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (convert(nvarchar(19),wbScanTime,120)>='{0}' and convert(nvarchar(19),wbScanTime,120)<='{1}') ", Convert.ToDateTime(dBegin).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(dEnd).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (strOperator != "-99" && strOperator != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (wbOperator='{0}') ", strOperator);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (wbOperator='{0}') ", strOperator);
                }
            }

            if (EmpNO != "-99" && EmpNO != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (EmpNO='{0}') ", EmpNO);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (EmpNO='{0}') ", EmpNO);
                }
            }

            if (feeFlag != "-99" && feeFlag != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (FeeFlag={0}) ", feeFlag);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (FeeFlag={0}) ", feeFlag);
                }
            }

            if (exceptionFlag != "-99" && exceptionFlag != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (ExceptionFlag={0}) ", exceptionFlag);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (ExceptionFlag={0}) ", exceptionFlag);
                }
            }
            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {

                }
                else
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (GroupID in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   (GroupID in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                }
            }
            else
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (GroupID in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (GroupID in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
            }
            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            //wbName,wbNumber,wbActualWeight,wbVolume,wbScanTime,wbOperator,wbWeightByVolume,FeeFlag,
            //FeeFlagDesc,ExceptionFlag,ExceptionFlagDesc,wbWidth,wbLength,wbHeight,mMemo,wbId
            dtCustom.Columns.Add("wbName", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbNumber", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbActualWeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbVolume", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbScanTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbOperator", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbWeightByVolume", Type.GetType("System.String"));
            dtCustom.Columns.Add("FeeFlag", Type.GetType("System.String"));
            dtCustom.Columns.Add("FeeFlagDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("ExceptionFlag", Type.GetType("System.String"));
            dtCustom.Columns.Add("ExceptionFlagDesc", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbWidth", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbLength", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbHeight", Type.GetType("System.String"));
            dtCustom.Columns.Add("mMemo", Type.GetType("System.String"));
            dtCustom.Columns.Add("GroupID", Type.GetType("System.String"));
            dtCustom.Columns.Add("GroupName", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpNO", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpName", Type.GetType("System.String"));
            dtCustom.Columns.Add("wbId", Type.GetType("System.String"));

            DataRow drCustom = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCustom = dtCustom.NewRow();

                string[] strFiledArray = strFileds.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        default:
                            drCustom[strFiledArray[j]] = dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""));
                            break;
                    }

                }
                if (drCustom["wbId"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("QueryExceptionWayBill_DS", dtCustom);

            localReport.DataSources.Add(reportDataSource);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = localReport.Render(
               "Excel", null, out mimeType, out encoding, out extension,
               out streamids, out warnings);
            string strFileName = Server.MapPath(STR_TEMPLATE_EXCEL);
            FileStream fs = new FileStream(strFileName, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            string strOutputFileName = LangHelper.GetLangbyKey("QueryExceptionWayBill_Control_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            switch (browserType.ToLower())
            {
                case "safari":
                    break;
                case "mozilla":
                    break;
                default:
                    strOutputFileName = HttpUtility.UrlEncode(strOutputFileName);
                    break;
            }

            return File(strFileName, "application/vnd.ms-excel", strOutputFileName);
        }
    }
}
