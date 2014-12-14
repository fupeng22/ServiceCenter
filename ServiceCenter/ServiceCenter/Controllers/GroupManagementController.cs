using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using SQLDAL;
using Model;
using ServiceCenter.Filter;
using Util;
using ServiceCenter.Models;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class GroupManagementController : Controller
    {
        //
        // GET: /GroupManagement/
        [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetData(string state)
        {
            string strResult = "";
            StringBuilder sb = new StringBuilder("");
            T_Group t_Group = null;
            DataSet ds = null;
            DataTable dt = null;

            t_Group = new T_Group();

            if (Session["Global_UserName"] != null)
            {
                if (ConstVariable.USERNAMEARRAY_ADMIN.Contains(Session["Global_UserName"].ToString().ToLower()))
                {
                    ds = t_Group.GetAllTopGroup();
                }
                else
                {
                    ds = t_Group.GetGroupByGroupId(Session["Global_GroupId"]);
                }
            }
            else
            {
                ds = t_Group.GetGroupByGroupId(Session["Global_GroupId"]);
            }
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    sb.Append("[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"text\":\"{0}\",\"state\":\"open\",\"id\":\"{1}\",", dt.Rows[i]["GroupName"].ToString(), dt.Rows[i]["ID"].ToString());
                        CreateGroupJSON(dt.Rows[i]["ID"].ToString(), ref sb, state);
                        if (sb.ToString().EndsWith(","))
                        {
                            sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
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
                    sb.Append("]");

                }
            }

            strResult = sb.ToString();
            return strResult;
        }

        protected string CreateGroupJSON(string GroupId, ref StringBuilder sb, string state)
        {
            string strResult = "";
            DataSet ds = null;
            DataTable dt = null;

            ds = new T_Group().GetGroupByUpGroupId(GroupId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    sb.Append("\"children\":[");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"text\":\"{0}\",\"state\":\"" + state + "\",\"id\":\"{1}\",", dt.Rows[i]["GroupName"].ToString(), dt.Rows[i]["ID"].ToString());
                        CreateGroupJSON(dt.Rows[i]["ID"].ToString(), ref sb, state);
                        if (sb.ToString().EndsWith(","))
                        {
                            sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
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

                    sb.Append("]");
                }
            }
            return strResult;
        }

        /// <summary>
        /// 判断部门名称是否使用（针对新建顶级部门的判断）
        /// </summary>
        /// <param name="departName"></param>
        /// <returns></returns>
        [HttpPost]
        public string TestExistDepartName(string departName)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip1") + "\"}";
            departName = Server.UrlDecode(departName);
            if (new T_Group().TestExistGroupName(departName))
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip2") + "\"}";
            }
            else
            {
                strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip3") + "\"}";
            }
            return strRet;
        }

        /// <summary>
        /// 判断此部门名称是否已经被其他的部门所使用(用于修改)
        /// </summary>
        /// <param name="departName"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        [HttpPost]
        public string TestAlreadyUsedDepartName(string departName, string departId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip1") + "\"}";
            departName = Server.UrlDecode(departName);
            departId = Server.UrlDecode(departId);
            if (new T_Group().TestExistGroupName(departName, departId))
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip2") + "\"}";
            }
            else
            {
                strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip3") + "\"}";
            }
            return strRet;
        }

        /// <summary>
        /// 判断是否是父部门（下面有子部门存在）
        /// </summary>
        /// <param name="departName"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        [HttpPost]
        public string TestIsSubDepart(string departId)
        {
            string strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip4") + "\"}";

            DataSet dsParentDept = null;
            DataTable dtParentDept = null;
            try
            {
                departId = Server.UrlDecode(departId);
                dsParentDept = new T_Group().GetGroupByUpGroupId(departId);
                if (dsParentDept != null)
                {
                    dtParentDept = dsParentDept.Tables[0];
                    if (dtParentDept != null && dtParentDept.Rows.Count > 0)
                    {
                        strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip5") + "\"}";
                    }
                }

            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip6") + ":" + ex.Message + "\"}";
            }

            return strRet;
        }

        /// <summary>
        /// 添加子部门的action
        /// </summary>
        /// <param name="departName"></param>
        /// <param name="parentDepartId"></param>
        /// <returns></returns>
        [HttpPost]
        public string InsertDepartment(string departName, string parentDepartId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip7") + "\"}";
            M_T_Group m_Group = new M_T_Group();

            try
            {
                departName = Server.UrlDecode(departName);
                parentDepartId = Server.UrlDecode(parentDepartId);

                m_Group.GroupName = departName;
                m_Group.GroupUpID = Convert.ToInt32(parentDepartId);

                if (new T_Group().InsertGroup(m_Group))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip8") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip9") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip10") + ":" + ex.Message + "\"}";
            }

            return strRet;
        }

        /// <summary>
        /// 添加顶级部门
        /// </summary>
        /// <param name="departName"></param>
        /// <param name="parentDepartId"></param>
        /// <returns></returns>
        [HttpPost]
        public string InsertTopDepartment(string departName)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip11") + "\"}";
            M_T_Group m_Group = new M_T_Group();

            try
            {
                departName = Server.UrlDecode(departName);

                m_Group.GroupName = departName;
                m_Group.GroupUpID = -1;

                if (new T_Group().InsertGroup(m_Group))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip12") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip13") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip14") + ":" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string UpdateDepartment(string newDepartName, string departId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip15") + "\"}";

            M_T_Group m_Group = new M_T_Group();

            try
            {
                newDepartName = Server.UrlDecode(newDepartName);
                departId = Server.UrlDecode(departId);

                m_Group.GroupName = newDepartName;
                m_Group.ID = Convert.ToInt32(departId);
                if (new T_Group().UpdateGroup(m_Group))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip16") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip17") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip18") + ":" + ex.Message + "\"}";
            }

            return strRet;
        }

        [HttpPost]
        public string DeleDepartment(string departId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip19") + "\"}";

            departId = Server.UrlDecode(departId);
            M_T_Group m_Group = new M_T_Group();

            try
            {
                m_Group.ID = Convert.ToInt32(departId);
                if (new T_Group().DeleGroup(m_Group))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip20") + "\"}";
                }
                else
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip21") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("GroupManagement_Control_Tip22") + ":" + ex.Message + "\"}";
            }

            return strRet;
        }
    }
}
