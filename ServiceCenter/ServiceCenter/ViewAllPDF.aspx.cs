using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

namespace ServiceCenter
{
    public partial class ViewAllPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strPDFFileId = Request.QueryString["pdfID"];
                if (string.IsNullOrEmpty(strPDFFileId))
                {
                    Response.Write("<script language=javascript>alert('参数错误')</script>");
                    return;
                }
                string strSql = @"SELECT  *
                                FROM    dbo.PDF_INFO
                                WHERE   PDF_LABEL = ( SELECT    PDF_LABEL
                                                      FROM      dbo.PDF_INFO
                                                      WHERE     PDF_ID = ";
                strSql = strSql + strPDFFileId + ") order by PDF_Create_Time desc";
                DataSet ds = null;
                ds = SqlServerHelper.Query(strSql);
                dl_AllPdfIFrame.DataSource = ds;
                dl_AllPdfIFrame.DataBind();
            }
        }
    }
}