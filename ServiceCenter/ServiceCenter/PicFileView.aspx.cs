using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

namespace XRayPictureView
{
    public partial class PicFileView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strPDFFileId = Request.QueryString["PDF_ID"];
                if (string.IsNullOrEmpty(strPDFFileId))
                {
                    Response.Write("<script language=javascript>alert('Parameter error')</script>");
                    return;
                }

                string strSql = @"select * from PDF_INFO where PDF_ID=" + strPDFFileId;
                DataSet ds = null;
                DataTable dt = null;
                ds = SqlServerHelper.Query(strSql);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count >= 0)
                    {
                        img_Pic.ImageUrl = "~/files" + ConvertToPicPath(dt.Rows[0]["PDF_FILE"].ToString());
                        linkImg.HRef = "~/files" + ConvertToPicPath(dt.Rows[0]["PDF_FILE"].ToString());
                    }
                }
            }
        }

        //\23.157.211.231\34\201407\pdf\ch18_00010000597089652480000022204_c.pdf
        private string ConvertToPicPath(string PdfFile)
        {
            string strRet = "";
            string[] strSource = PdfFile.Split('\\');
            for (int i = 0; i < strSource.Length; i++)
            {
                if (strSource[i].ToLower() == "pdf")
                {
                    strSource[i] = "pic";
                }
                if (i == strSource.Length - 1)
                {
                    strSource[i] = strSource[i].ToLower().Replace(".pdf", ".jpg");
                }
            }
            for (int i = 0; i < strSource.Length; i++)
            {
                strRet = strRet + "\\" + strSource[i];
            }
            strRet = strRet.Replace("\\\\", "\\");
            return strRet;
        }
    }
}