using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using ServiceCenter.Models;

namespace ServiceCenter
{
    public partial class PicFileView_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strPicFileId = Request.QueryString["PIC_ID"];
                if (string.IsNullOrEmpty(strPicFileId))
                {
                    Response.Write("<script language=javascript>alert('" + LangHelper.GetLangbyKey("PicFileView_New_Aspx_cs_InvalidParam") + "')</script>");
                    return;
                }

                string strSql = @"select * from T_PicInfo where PIC_ID=" + strPicFileId;
                DataSet ds = null;
                DataTable dt = null;
                ds = SqlServerHelper.Query(strSql);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count >= 0)
                    {
                        img_Pic.ImageUrl = "~/files" + dt.Rows[0]["PIC_PC_FileName"].ToString();
                        linkImg.HRef = "~/files" + dt.Rows[0]["PIC_PC_FileName"].ToString();
                    }
                }
            }
        }
    }
}