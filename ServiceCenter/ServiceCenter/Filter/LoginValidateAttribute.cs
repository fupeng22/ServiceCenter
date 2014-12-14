using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceCenter.Models;

namespace ServiceCenter.Filter
{
    public class LoginValidateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在action method之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;
            if (filterContext.HttpContext.Session["Global_UserName"] == null)
            {
                filterContext.HttpContext.Response.Redirect("/" + LangHelper.GetWhichLang() + "/Login/Index" + "?URLRet=" + redirectOnSuccess);
            }
        }
    }
}