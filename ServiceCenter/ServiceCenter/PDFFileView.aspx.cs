using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.IO;
using System.Diagnostics;

namespace XRayPictureView
{
    public partial class PDFFileView : System.Web.UI.Page
    {
        const string STR_SWF_FOLDER = "~/files/tmp/";
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
                        //ShowPdf1.FilePath ="/files"+ dt.Rows[0]["PDF_FILE"].ToString();
                       pub_SWFFileName= ConvertPDFToSWF(Server.MapPath("~/files/") + dt.Rows[0]["PDF_FILE"].ToString());
                    }
                }
            }
        }

        protected string ConvertPDFToSWF(string strPDFFile)
        {
            string strSWFFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".swf";
            string strSWFFile = Server.MapPath(STR_SWF_FOLDER) + strSWFFileName;
            string cmdStr = HttpContext.Current.Server.MapPath("~/SWFTools/pdf2swf.exe");
            string sourcePath = @"""" + strPDFFile + @"""";
            string targetPath = @"""" + strSWFFile + @"""";
            string argsStr = "  -t " + sourcePath + " -s flashversion=9 -o " + targetPath;
            ExcutedCmd(cmdStr, argsStr);
            return strSWFFileName;
        }

        /// <summary>
        /// 转换成 SWF 文件
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="args">命令参数</param>
        private static void ExcutedCmd(string cmd, string args)
        {
            using (Process p = new Process())
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmd, args.Replace("\"", ""));
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();
            }
        }

        public string pub_SWFFileName
        {
            get;
            set;
        }
    }
}