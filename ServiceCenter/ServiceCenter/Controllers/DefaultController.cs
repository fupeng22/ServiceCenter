using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceCenter.Filter;
using System.Text;
using System.Data;
using SQLDAL;
using ServiceCenter.Models;
using Util;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class DefaultController : Controller
    {
        //
        // GET: /Default1/
        [LoginValidate]
        public ActionResult Index()
        {
            //throw new Exception("出错啦");
            return View();
        }

        [HttpPost]
        public string LoadStations()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new T_ScanLog().GetAllStations();
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---请选择---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultSelect"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["wbOperator"].ToString(), dt.Rows[i]["wbOperator"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadAllEmp()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null; 
            DataTable dt = null; new DataTable();

            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {
                    ds = new T_Employee().GetEmp();
                }
                else
                {
                    ds = new T_Employee().GetEmp(Session["Global_GroupId"].ToString());
                }
            }
            else
            {
                ds = new T_Employee().GetEmp(Session["Global_GroupId"].ToString());
            }

            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---请选择---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultSelect"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["EmpNO"].ToString(), dt.Rows[i]["EmpName"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadAllNVR()
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = new  T_NVR_INFO().GetAllNVR();
            DataTable dt = new DataTable();
            sb.Append("[");
            sb.Append("{");
            //sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", "---请选择---");
            sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", "-99", LangHelper.GetLangbyKey("Public_Select_DefaultSelect"));
            sb.Append("},");
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"id\":\"{0}\",\"text\":\"{1}\"", dt.Rows[i]["NVR_ID"].ToString(), dt.Rows[i]["NVR_NAME"].ToString());
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append("},");
                        }
                        else
                        {
                            sb.Append("}");
                        }
                    }
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Remove(sb.ToString().Length - 1));
            }
            sb.Append("]");
            strResult = sb.ToString();
            return strResult;
        }

        [HttpPost]
        public string LoadFeeTypeJSON()
        {
            //return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"按实际重量收费\"},{\"id\":\"1\",\"text\":\"按体积重量收费\"}]";
            return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultSelect") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_FeeType1") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_FeeType2") + "\"}]";
        }

        [HttpPost]
        public string LoadExceptionTypeJSON()
        {
            //return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"无异常\"},{\"id\":\"1\",\"text\":\"异常\"}]";
            return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultSelect") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ExceptionType1") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ExceptionType2") + "\"}]";
        }

        [HttpPost]
        public string LoadConnTypeJSON()
        {
            //return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"232直连\"},{\"id\":\"1\",\"text\":\"232转tcp/ip\"},{\"id\":\"2\",\"text\":\"485直连\"},{\"id\":\"3\",\"text\":\"485转tcp/ip\"},{\"id\":\"4\",\"text\":\"设备直发\"}]";
            return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultSelect") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType1") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType2") + "\"},{\"id\":\"2\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType3") + "\"},{\"id\":\"3\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType4") + "\"},{\"id\":\"4\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType5") + "\"}]";
        }

        [HttpPost]
        public string LoadXRayTypeJSON()
        {
            return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"Inner\"},{\"id\":\"1\",\"text\":\"Outside the entrance of the X-ray machine\"}]";
            //return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultSelect") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType1") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType2") + "\"},{\"id\":\"2\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType3") + "\"},{\"id\":\"3\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType4") + "\"},{\"id\":\"4\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType5") + "\"}]";
        }

        [HttpPost]
        public string LoadXRayDirectionJSON()
        {
            return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"From left to right\"},{\"id\":\"1\",\"text\":\"From right to left\"}]";
            //return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultSelect") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType1") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType2") + "\"},{\"id\":\"2\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType3") + "\"},{\"id\":\"3\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType4") + "\"},{\"id\":\"4\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType5") + "\"}]";
        }

        [HttpPost]
        public string LoadXRayUseFlagJSON()
        {
            return "[{\"id\":\"-99\",\"text\":\"--请选择--\",\"selected\":true},{\"id\":\"0\",\"text\":\"N\"},{\"id\":\"1\",\"text\":\"Y\"}]";
            //return "[{\"id\":\"-99\",\"text\":\"" + LangHelper.GetLangbyKey("Public_Select_DefaultSelect") + "\",\"selected\":true},{\"id\":\"0\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType1") + "\"},{\"id\":\"1\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType2") + "\"},{\"id\":\"2\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType3") + "\"},{\"id\":\"3\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType4") + "\"},{\"id\":\"4\",\"text\":\"" + LangHelper.GetLangbyKey("Default_Control_ConnType5") + "\"}]";
        }
    }
}
