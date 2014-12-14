using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using Util;
using System.Data;
using ServiceCenter.Filter;
using ServiceCenter.Models;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Login(string strUserName, string strUserPwd)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Login_Control_ErrorTip1") + "\"}";
            DataSet ds = null;
            DataTable dt = null;
            strUserName = Server.UrlDecode(strUserName);
            strUserPwd = Server.UrlDecode(strUserPwd);
            try
            {
                if (new T_Users().Login(strUserName, MD5Helper.GetMD5(strUserPwd)))
                {
                    Session["Global_UserName"] = strUserName;
                    
                    ds = new T_Users().GetUseByUsername(strUserName);
                    if (ds!=null)
                    {
                        dt = ds.Tables[0];
                        if (dt!=null && dt.Rows.Count>0)
                        {
                            //保存登陆用户的信息
                            Session["Global_GroupName"] = dt.Rows[0]["GroupName"].ToString();
                            Session["Global_GroupId"] = dt.Rows[0]["Group_ID"].ToString();
                            Session.Timeout=60*8;
                        }
                    }
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("Login_Control_ErrorTip2") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Login_Control_ErrorTip3") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Login_Control_ErrorTip4") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

        [HttpPost]
        public string Logout()
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Login_Control_ErrorTip5") + "\"}";
          
            try
            {
                Session["Global_UserName"] = null;
                Session["Global_GroupName"] = null;
                strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("Login_Control_ErrorTip6") + "\"}";
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Login_Control_ErrorTip7") + ":" + ex.Message + "\"}";
            }
            return strRet;
        }

    }
}
