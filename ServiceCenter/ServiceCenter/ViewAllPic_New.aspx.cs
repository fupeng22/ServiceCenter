using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.Data;
using ServiceCenter.Models;

namespace ServiceCenter
{
    public partial class ViewAllPic_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strPicFileId = Request.QueryString["PicID"];
                if (string.IsNullOrEmpty(strPicFileId))
                {
                    Response.Write("<script language=javascript>alert('" + LangHelper.GetLangbyKey("ViewAllPic_New_Aspx_cs_InvalidParam") + "')</script>");
                    return;
                }
                string strSql = @"SELECT  *
                                FROM    T_PicInfo
                                WHERE   PIC_Label = ( SELECT    PIC_Label
                                                      FROM      T_PicInfo
                                                      WHERE     PIC_ID = ";
                strSql = strSql + strPicFileId + ") order by PIC_Time desc";
                DataSet ds = null;
                DataTable dt = null;
                ds = SqlServerHelper.Query(strSql);
                dl_AllPicIFrame.DataSource = ds;
                dl_AllPicIFrame.DataBind();
            }
        }
    }
}