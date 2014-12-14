using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using ServiceCenter.Filter;
using System.Data.SqlClient;
using System.Data;
using Util;
using DBUtility;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.IO;
using ServiceCenter.Models;
using Model;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class EmployeeManagementController : Controller
    {
        T_Employee tEmployee = new T_Employee();

        public const string strFileds = "EmpNO,EmpName,EmpDepartment,GroupName,cId";

        public const string STR_TEMPLATE_EXCEL = "~/Temp/Template/template.xls";
        public const string STR_REPORT_URL = "~/Content/Reports/Employee.rdlc";
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
            param[0].Value = "V_Employee";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "cId";

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
            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {

                }
                else
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (EmpDepartment in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   (EmpDepartment in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                }
            }
            else
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (EmpDepartment in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (EmpDepartment in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
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
        public ActionResult Print(string order, string page, string rows, string sort)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_Employee";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "cId";

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
            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {

                }
                else
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (EmpDepartment in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   (EmpDepartment in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                }
            }
            else
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (EmpDepartment in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (EmpDepartment in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
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
            //EmpNO,EmpName,EmpDepartment,GroupName,cId
            dtCustom.Columns.Add("EmpNO", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpName", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpDepartment", Type.GetType("System.String"));
            dtCustom.Columns.Add("GroupName", Type.GetType("System.String"));
            dtCustom.Columns.Add("cId", Type.GetType("System.String"));

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
                if (drCustom["cId"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("Employee_DS", dtCustom);

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
            param[0].Value = "V_Employee";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "cId";

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
            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {

                }
                else
                {
                    if (strWhereTemp != "")
                    {
                        strWhereTemp = strWhereTemp + string.Format(" and (EmpDepartment in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                    else
                    {
                        strWhereTemp = strWhereTemp + string.Format("   (EmpDepartment in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                    }
                }
            }
            else
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + string.Format(" and (EmpDepartment in ({0})) ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
                }
                else
                {
                    strWhereTemp = strWhereTemp + string.Format("   (EmpDepartment in ({0}))  ", new T_Group().GetSubDepartIdsByDepartId(Session["Global_GroupId"]));
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
            //EmpNO,EmpName,EmpDepartment,GroupName,cId
            dtCustom.Columns.Add("EmpNO", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpName", Type.GetType("System.String"));
            dtCustom.Columns.Add("EmpDepartment", Type.GetType("System.String"));
            dtCustom.Columns.Add("GroupName", Type.GetType("System.String"));
            dtCustom.Columns.Add("cId", Type.GetType("System.String"));

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
                if (drCustom["cId"].ToString() != "")
                {
                    dtCustom.Rows.Add(drCustom);
                }
            }
            dt = null;
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(STR_REPORT_URL);
            ReportDataSource reportDataSource = new ReportDataSource("Employee_DS", dtCustom);

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

            string strOutputFileName = LangHelper.GetLangbyKey("EmployeeManagement_Control_ExcelName") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

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
        public string AddEmployee(string EmpNO, string EmpName, string EmpDepartment)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip1") + "\"}";
            M_Employee m_Employee = new M_Employee();
            EmpNO = Server.UrlDecode(EmpNO);
            EmpName = Server.UrlDecode(EmpName);
            EmpDepartment = Server.UrlDecode(EmpDepartment);
            m_Employee.EmpNO = EmpNO;
            m_Employee.EmpName = EmpName;
            m_Employee.EmpDepartment = Convert.ToInt32(EmpDepartment);

            try
            {
                if ((new  T_Employee()).addEmployee(m_Employee))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip2") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip4") + "：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateEmployee(string EmpId, string EmpNO, string EmpName, string EmpDepartment)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip5") + "\"}";
            M_Employee m_Employee = new M_Employee();
            EmpId = Server.UrlDecode(EmpId);
            EmpNO = Server.UrlDecode(EmpNO);
            EmpName = Server.UrlDecode(EmpName);
            EmpDepartment = Server.UrlDecode(EmpDepartment);
            m_Employee.EmpNO = EmpNO;
            m_Employee.EmpName = EmpName;
            m_Employee.EmpDepartment = Convert.ToInt32(EmpDepartment);
            m_Employee.cId = Convert.ToInt32(EmpId);
            try
            {
                if ((new  T_Employee()).updateEmployee(m_Employee))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip6") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip7") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip8") + "：" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string Delete(string ids)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip9") + "\"}";
            //string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + "\"}";
            ids = Server.UrlDecode(ids);
            try
            {
                if (tEmployee .deleteEmployee(ids))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip10") + "\"}";
                    //      strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip11") + "\"}";
                    //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip12") + ":" + ex.Message + "\"}";
                //strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("User_Controller_ErrorMessage2") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpGet]
        public string ExistEmployeeNO(string strEmpNo)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            strEmpNo = Server.UrlDecode(strEmpNo);
            try
            {

                if (tEmployee.EmployeeNOExist(strEmpNo))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip13") + "\"}";
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
        public string ExistEmployeeNO_Update(string id, string strEmpNo)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"\"}";
            id = Server.UrlDecode(id);
            strEmpNo = Server.UrlDecode(strEmpNo);
            try
            {

                if (tEmployee.EmployeeNOExist(Convert.ToInt32(id), strEmpNo))
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("EmployeeManagement_Control_ErrorTip13") + "\"}";
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
