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
using ServiceCenter.Filter;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class QueryPDFController : Controller
    {
        public const string strFileds = "PDF_LABEL,PDF_FILE,PDF_FILE_TYPE,PDF_FILE_TYPE_Desc,PDF_Create_Time,PDF_ID";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/QueryPDF.rdlc";
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
        public string GetData(string order, string page, string rows, string sort, string beginDT, string endDT, string PdfLabel)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_PDF_INFO";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "PDF_ID";

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
            PdfLabel = Server.UrlDecode(PdfLabel);
            beginDT = Server.UrlDecode(beginDT);
            endDT = Server.UrlDecode(endDT);

            if (beginDT != "" && endDT != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (convert(nvarchar(19),PDF_Create_Time,120)>='{0}' and convert(nvarchar(19),PDF_Create_Time,120)<='{1}') ", Convert.ToDateTime(beginDT).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endDT).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (convert(nvarchar(19),PDF_Create_Time,120)>='{0}' and convert(nvarchar(19),PDF_Create_Time,120)<='{1}') ", Convert.ToDateTime(beginDT).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endDT).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (PdfLabel != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (PDF_LABEL like '%{0}%') ", PdfLabel);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (PDF_LABEL like '%{0}%') ", PdfLabel);
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
                        case "PDF_LABEL":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], (dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""))).Replace('\'','‘'));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], (dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""))).Replace('\'','‘'));
                            }
                            break;
                        case "PDF_FILE":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], (dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""))).Replace('\\', '/'));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], (dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\r\n", ""))).Replace('\\', '/'));
                            }
                            break;
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
        public ActionResult Print(string order, string page, string rows, string sort, string beginDT, string endDT, string PdfLabel)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_PDF_INFO";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "PDF_ID";

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
            PdfLabel = Server.UrlDecode(PdfLabel);
            beginDT = Server.UrlDecode(beginDT);
            endDT = Server.UrlDecode(endDT);

            if (beginDT != "" && endDT != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (convert(nvarchar(19),PDF_Create_Time,120)>='{0}' and convert(nvarchar(19),PDF_Create_Time,120)<='{1}') ", Convert.ToDateTime(beginDT).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endDT).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (convert(nvarchar(19),PDF_Create_Time,120)>='{0}' and convert(nvarchar(19),PDF_Create_Time,120)<='{1}') ", Convert.ToDateTime(beginDT).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endDT).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (PdfLabel != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (PDF_LABEL like '%{0}%') ", PdfLabel);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (PDF_LABEL like '%{0}%') ", PdfLabel);
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
            //PDF_LABEL,PDF_FILE,PDF_FILE_TYPE,PDF_FILE_TYPE_Desc,PDF_Create_Time,PDF_ID
            dtCustom.Columns.Add("PDF_LABEL", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_FILE", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_FILE_TYPE", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_FILE_TYPE_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_Create_Time", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_ID", Type.GetType("System.String"));

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
                if (drCustom["PDF_ID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("QueryPDF_DS", dtCustom);

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
        public ActionResult Excel(string order, string page, string rows, string sort, string beginDT, string endDT, string PdfLabel, string browserType)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_PDF_INFO";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "PDF_ID";

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
            PdfLabel = Server.UrlDecode(PdfLabel);
            beginDT = Server.UrlDecode(beginDT);
            endDT = Server.UrlDecode(endDT);

            if (beginDT != "" && endDT != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (convert(nvarchar(19),PDF_Create_Time,120)>='{0}' and convert(nvarchar(19),PDF_Create_Time,120)<='{1}') ", Convert.ToDateTime(beginDT).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endDT).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("  (convert(nvarchar(19),PDF_Create_Time,120)>='{0}' and convert(nvarchar(19),PDF_Create_Time,120)<='{1}') ", Convert.ToDateTime(beginDT).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endDT).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (PdfLabel != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (PDF_LABEL like '%{0}%') ", PdfLabel);
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (PDF_LABEL like '%{0}%') ", PdfLabel);
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
            //NVR_NAME,NVR_IP,NVR_PORT,NVR_USER,NVR_PW,NVR_BAK,NVR_ID
            dtCustom.Columns.Add("PDF_LABEL", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_FILE", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_FILE_TYPE", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_FILE_TYPE_Desc", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_Create_Time", Type.GetType("System.String"));
            dtCustom.Columns.Add("PDF_ID", Type.GetType("System.String"));

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
                if (drCustom["PDF_ID"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("QueryPDF_DS", dtCustom);

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

            string strOutputFileName = "图片查询信息" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
