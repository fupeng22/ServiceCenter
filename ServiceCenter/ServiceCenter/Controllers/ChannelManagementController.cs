using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using Model;
using ServiceCenter.Filter;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class ChannelManagementController : Controller
    {
        T_XRayInfo tXRayInfo = new T_XRayInfo();

        public const string strFileds = "XRAY_NVR_ID,XRAY_TYPE,XRAY_NAME,NVR_NAME,XRAY_TYPE_Desc,BEGIN_X,BEGIN_Y,END_Y,RUN_DIRECTOR,USE_FLAG,EVERY_INTERNAL,RUN_DIRECTOR_Desc,USE_FLAG_Desc,LastCheckTime,BeginCheckTime,XRAY_CHANNEL_NO,CRITICAL_VALUE,XRAY_ID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/ChannelManagement.rdlc";
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
        public string GetData(string order, string page, string rows, string sort)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_XRay_Info";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "XRAY_ID";

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
        public ActionResult Print(string order, string page, string rows, string sort)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_XRay_Info";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "XRAY_ID";

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

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();

            dtCustom.Columns.Add("XRAY_NVR_ID", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_TYPE", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_NAME", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_NAME", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_TYPE_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("BEGIN_X", Type.GetType("System.String"));
            dtCustom.Columns.Add("BEGIN_Y", Type.GetType("System.String"));
            dtCustom.Columns.Add("END_Y", Type.GetType("System.String"));
            dtCustom.Columns.Add("RUN_DIRECTOR", Type.GetType("System.String"));
            dtCustom.Columns.Add("USE_FLAG", Type.GetType("System.String"));
            dtCustom.Columns.Add("EVERY_INTERNAL", Type.GetType("System.String"));
            dtCustom.Columns.Add("RUN_DIRECTOR_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("USE_FLAG_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("LastCheckTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("BeginCheckTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_CHANNEL_NO", Type.GetType("System.String"));
            dtCustom.Columns.Add("CRITICAL_VALUE", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_ID", Type.GetType("System.String"));

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
                if (drCustom["XRAY_ID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("ChannelManagement_DS", dtCustom);

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
        public ActionResult Excel(string order, string page, string rows, string sort, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_XRay_Info";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "XRAY_ID";

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

            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            dtCustom.Columns.Add("XRAY_NVR_ID", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_TYPE", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_NAME", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_NAME", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_TYPE_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("BEGIN_X", Type.GetType("System.String"));
            dtCustom.Columns.Add("BEGIN_Y", Type.GetType("System.String"));
            dtCustom.Columns.Add("END_Y", Type.GetType("System.String"));
            dtCustom.Columns.Add("RUN_DIRECTOR", Type.GetType("System.String"));
            dtCustom.Columns.Add("USE_FLAG", Type.GetType("System.String"));
            dtCustom.Columns.Add("EVERY_INTERNAL", Type.GetType("System.String"));
            dtCustom.Columns.Add("RUN_DIRECTOR_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("USE_FLAG_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("LastCheckTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("BeginCheckTime", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_CHANNEL_NO", Type.GetType("System.String"));
            dtCustom.Columns.Add("CRITICAL_VALUE", Type.GetType("System.String"));
            dtCustom.Columns.Add("XRAY_ID", Type.GetType("System.String"));

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
                if (drCustom["XRAY_ID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("ChannelManagement_DS", dtCustom);

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

            string strOutputFileName = "通道信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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

        [HttpPost]
        public string AddChannel(string XRAY_NVR_ID, string XRAY_TYPE, string BEGIN_X, string BEGIN_Y, string END_Y, string RUN_DIRECTOR, string CRITICAL_VALUE, string USE_FLAG, string XRAY_CHANNEL_NO, string XRAY_NAME, string BeginCheckTime, string LastCheckTime, string EVERY_INTERNAL)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + "添加失败，原因未知" + "\"}";
            M_XRayInfo mXRayInfo = new M_XRayInfo();
            XRAY_NVR_ID = Server.UrlDecode(XRAY_NVR_ID);
            XRAY_TYPE = Server.UrlDecode(XRAY_TYPE);
            BEGIN_X = Server.UrlDecode(BEGIN_X);
            BEGIN_Y = Server.UrlDecode(BEGIN_Y);
            END_Y = Server.UrlDecode(END_Y);
            RUN_DIRECTOR = Server.UrlDecode(RUN_DIRECTOR);
            CRITICAL_VALUE = Server.UrlDecode(CRITICAL_VALUE);
            USE_FLAG = Server.UrlDecode(USE_FLAG);
            XRAY_CHANNEL_NO = Server.UrlDecode(XRAY_CHANNEL_NO);
            XRAY_NAME = Server.UrlDecode(XRAY_NAME);
            BeginCheckTime = Server.UrlDecode(BeginCheckTime);
            LastCheckTime = Server.UrlDecode(LastCheckTime);
            EVERY_INTERNAL = Server.UrlDecode(EVERY_INTERNAL);

            mXRayInfo.XRAY_NVR_ID = Convert.ToInt32(XRAY_NVR_ID);
            mXRayInfo.XRAY_TYPE = Convert.ToInt32(XRAY_TYPE);
            mXRayInfo.BEGIN_X = Convert.ToInt32(BEGIN_X);
            mXRayInfo.BEGIN_Y = Convert.ToInt32(BEGIN_Y);
            mXRayInfo.END_Y = Convert.ToInt32(END_Y);
            mXRayInfo.RUN_DIRECTOR = Convert.ToInt32(RUN_DIRECTOR);
            mXRayInfo.CRITICAL_VALUE = Convert.ToInt32(CRITICAL_VALUE);
            mXRayInfo.USE_FLAG = Convert.ToInt32(USE_FLAG);
            mXRayInfo.XRAY_CHANNEL_NO = Convert.ToInt32(XRAY_CHANNEL_NO);
            mXRayInfo.XRAY_NAME = XRAY_NAME;
            mXRayInfo.BeginCheckTime = Convert.ToDateTime(Convert.ToDateTime(BeginCheckTime).ToString("yyyy-MM-dd HH:mm:ss"));
            mXRayInfo.LastCheckTime = Convert.ToDateTime(Convert.ToDateTime(LastCheckTime).ToString("yyyy-MM-dd HH:mm:ss"));
            mXRayInfo.EVERY_INTERNAL = Convert.ToInt32(EVERY_INTERNAL);

            try
            {
                if ((new  T_XRayInfo()).addXRayInfo(mXRayInfo))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "添加成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "添加失败" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + "添加失败，原因" + "：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateChannel(string XRAY_ID, string XRAY_NVR_ID, string XRAY_TYPE, string BEGIN_X, string BEGIN_Y, string END_Y, string RUN_DIRECTOR, string CRITICAL_VALUE, string USE_FLAG, string XRAY_CHANNEL_NO, string XRAY_NAME, string BeginCheckTime, string LastCheckTime, string EVERY_INTERNAL)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + "修改失败，原因未知" + "\"}";
            M_XRayInfo mXRayInfo = new M_XRayInfo();
            XRAY_ID = Server.UrlDecode(XRAY_ID);
            XRAY_NVR_ID = Server.UrlDecode(XRAY_NVR_ID);
            XRAY_TYPE = Server.UrlDecode(XRAY_TYPE);
            BEGIN_X = Server.UrlDecode(BEGIN_X);
            BEGIN_Y = Server.UrlDecode(BEGIN_Y);
            END_Y = Server.UrlDecode(END_Y);
            RUN_DIRECTOR = Server.UrlDecode(RUN_DIRECTOR);
            CRITICAL_VALUE = Server.UrlDecode(CRITICAL_VALUE);
            USE_FLAG = Server.UrlDecode(USE_FLAG);
            XRAY_CHANNEL_NO = Server.UrlDecode(XRAY_CHANNEL_NO);
            XRAY_NAME = Server.UrlDecode(XRAY_NAME);
            BeginCheckTime = Server.UrlDecode(BeginCheckTime);
            LastCheckTime = Server.UrlDecode(LastCheckTime);
            EVERY_INTERNAL = Server.UrlDecode(EVERY_INTERNAL);

            mXRayInfo.XRAY_ID = Convert.ToInt32(XRAY_ID);
            mXRayInfo.XRAY_NVR_ID = Convert.ToInt32(XRAY_NVR_ID);
            mXRayInfo.XRAY_TYPE = Convert.ToInt32(XRAY_TYPE);
            mXRayInfo.BEGIN_X = Convert.ToInt32(BEGIN_X);
            mXRayInfo.BEGIN_Y = Convert.ToInt32(BEGIN_Y);
            mXRayInfo.END_Y = Convert.ToInt32(END_Y);
            mXRayInfo.RUN_DIRECTOR = Convert.ToInt32(RUN_DIRECTOR);
            mXRayInfo.CRITICAL_VALUE = Convert.ToInt32(CRITICAL_VALUE);
            mXRayInfo.USE_FLAG = Convert.ToInt32(USE_FLAG);
            mXRayInfo.XRAY_CHANNEL_NO = Convert.ToInt32(XRAY_CHANNEL_NO);
            mXRayInfo.XRAY_NAME = XRAY_NAME;
            mXRayInfo.BeginCheckTime = Convert.ToDateTime(Convert.ToDateTime(BeginCheckTime).ToString("yyyy-MM-dd HH:mm:ss"));
            mXRayInfo.LastCheckTime = Convert.ToDateTime(Convert.ToDateTime(LastCheckTime).ToString("yyyy-MM-dd HH:mm:ss"));
            mXRayInfo.EVERY_INTERNAL = Convert.ToInt32(EVERY_INTERNAL);
            try
            {
                if ((new T_XRayInfo()).updateXRayInfo(mXRayInfo))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "修改成功" + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "修改失败" + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + "修改失败，原因" + "：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string Delete(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + "删除失败，原因未知" + "\"}";
            //string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tXRayInfo.deleteXRayInfo(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + "删除成功" + "\"}";
                    //      strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "删除失败" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + "删除失败，原因" + ":" + ex.Message + "\"}";
                //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpGet]
        public string ExistXRayName(string strXRayName)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            strXRayName = Server.UrlDecode(strXRayName);
            try
            {

                if (tXRayInfo.XRayNameExist(strXRayName))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此通道名称已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;
        }


        [HttpGet]
        public string ExistXRayName_Update(string XRayId, string strXRayName)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            XRayId = Server.UrlDecode(XRayId);
            strXRayName = Server.UrlDecode(strXRayName);
            try
            {

                if (tXRayInfo.XRayNameExist(Convert.ToInt32(XRayId), strXRayName))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + "此通道名称已经使用" + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage1") + "\"}";
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + ex.Message + "\"}";
            }

            return strRet;

        }
    }
}
