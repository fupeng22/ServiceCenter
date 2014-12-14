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
using Microsoft.Reporting.WebForms;
using System.IO;
using Model;
using SQLDAL;
using ServiceCenter.Models;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class DeviceManagementController : Controller
    {
        public const string strFileds = "NVR_NAME,NVR_IP,NVR_PORT,NVR_USER,NVR_PW,NVR_BAK,NVR_DHL_IP,NVR_ID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/DeviceManagement.rdlc";
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
            param[0].Value = "NVR_INFO";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "NVR_ID";

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
            param[0].Value = "NVR_INFO";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "NVR_ID";

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
            string strWhereTemp = "";
            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            //NVR_NAME,NVR_IP,NVR_PORT,NVR_USER,NVR_PW,NVR_BAK,NVR_ID
            dtCustom.Columns.Add("NVR_NAME", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_IP", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_PORT", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_USER", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_PW", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_BAK", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_DHL_IP", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_ID", Type.GetType("System.String"));

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
                if (drCustom["NVR_ID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("DeviceManagement_DS", dtCustom);

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
            param[0].Value = "NVR_INFO";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "NVR_ID";

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
            string strWhereTemp = "";
            param[6].Value = strWhereTemp;

            param[7] = new SqlParameter();
            param[7].SqlDbType = SqlDbType.Int;
            param[7].ParameterName = "@RecordCount";
            param[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlServerHelper.RunProcedure("spPageViewByStr", param, "result");
            DataTable dt = ds.Tables["result"];
            DataTable dtCustom = new DataTable();
            //NVR_NAME,NVR_IP,NVR_PORT,NVR_USER,NVR_PW,NVR_BAK,NVR_ID
            dtCustom.Columns.Add("NVR_NAME", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_IP", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_PORT", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_USER", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_PW", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_BAK", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_DHL_IP", Type.GetType("System.String"));
            dtCustom.Columns.Add("NVR_ID", Type.GetType("System.String"));

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
                if (drCustom["NVR_ID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("DeviceManagement_DS", dtCustom);

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

            string strOutputFileName = LangHelper.GetLangbyKey("DeviceManagement_Control_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
        public string AddNVR(string NVR_NAME, string NVR_IP, string NVR_PORT, string NVR_USER, string NVR_PW, string NVR_BAK,string NVR_DHL_IP)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip1") + "\"}";
            M_NVR_INFO m_NVRInfo = new M_NVR_INFO();
            NVR_NAME = Server.UrlDecode(NVR_NAME);
            NVR_IP = Server.UrlDecode(NVR_IP);
            NVR_PORT = Server.UrlDecode(NVR_PORT);
            NVR_USER = Server.UrlDecode(NVR_USER);
            NVR_PW = Server.UrlDecode(NVR_PW);
            NVR_BAK = Server.UrlDecode(NVR_BAK);
            NVR_DHL_IP = Server.UrlDecode(NVR_DHL_IP);

            m_NVRInfo.NVR_Name = NVR_NAME;
            m_NVRInfo.NVR_Ip = NVR_IP;
            m_NVRInfo.NVR_Port = Convert.ToInt32(NVR_PORT);
            m_NVRInfo.NVR_User = NVR_USER;
            m_NVRInfo.NVR_Pw = NVR_PW;
            m_NVRInfo.NVR_Bak = NVR_BAK;
            m_NVRInfo.NVR_DHL_IP = NVR_DHL_IP;
            try
            {
                if ((new  T_NVR_INFO()).addNVR(m_NVRInfo))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip2") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip4") + "：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateNVR(string NVR_ID, string NVR_NAME, string NVR_IP, string NVR_PORT, string NVR_USER, string NVR_PW, string NVR_BAK, string NVR_DHL_IP)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip5") + "\"}";
            M_NVR_INFO m_NVRInfo = new M_NVR_INFO();
            NVR_ID = Server.UrlDecode(NVR_ID);
            NVR_NAME = Server.UrlDecode(NVR_NAME);
            NVR_IP = Server.UrlDecode(NVR_IP);
            NVR_PORT = Server.UrlDecode(NVR_PORT);
            NVR_USER = Server.UrlDecode(NVR_USER);
            NVR_PW = Server.UrlDecode(NVR_PW);
            NVR_BAK = Server.UrlDecode(NVR_BAK);
            NVR_DHL_IP = Server.UrlDecode(NVR_DHL_IP);

            m_NVRInfo.NVR_Name = NVR_NAME;
            m_NVRInfo.NVR_Ip = NVR_IP;
            m_NVRInfo.NVR_Port = Convert.ToInt32(NVR_PORT);
            m_NVRInfo.NVR_User = NVR_USER;
            m_NVRInfo.NVR_Pw = NVR_PW;
            m_NVRInfo.NVR_Bak = NVR_BAK;
            m_NVRInfo.NVR_DHL_IP = NVR_DHL_IP;
            m_NVRInfo.NVR_Id = Convert.ToInt32(NVR_ID);
            try
            {
                if ((new  T_NVR_INFO()).updateNVR(m_NVRInfo))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip6") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip7") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip8") + "：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string Delete(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip9") + "\"}";
            //string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (new  T_NVR_INFO().deleteNVR(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip10") + "\"}";
                    //      strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip11") + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip12") + ":" + ex.Message + "\"}";
                //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpGet]
        public string ExistNVRName(string strNVRName)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            strNVRName = Server.UrlDecode(strNVRName);
            try
            {

                if ((new  T_NVR_INFO()).NVRNameExist(strNVRName))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip13") + "\"}";
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
        public string ExistNVRName_Update(string id, string strNVRName)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            id = Server.UrlDecode(id);
            strNVRName = Server.UrlDecode(strNVRName);
            try
            {

                if ((new  T_NVR_INFO ()).NVRNameExist(Convert.ToInt32(id), strNVRName))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("DeviceManagement_Control_ErrorTip13") + "\"}";
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
