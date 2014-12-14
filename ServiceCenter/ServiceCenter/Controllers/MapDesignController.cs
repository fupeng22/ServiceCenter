using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DBUtility;
using System.IO;
using SQLDAL;
using ServiceCenter.Filter;
using ServiceCenter.Models;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class MapDesignController : Controller
    {
        public const string strFileds = "GroupId,MapName,MapPath,mMemo,GroupName,Id";
        public const string strFileds1 = "GroupId,MapName,MapDes,MapPath,mMemo,GroupName,Id";

        private const string STR_MAP_FOLDER = "~/images/map/imgs/";
        //
        // GET: /MapDesign/

         [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }

        public string GetData(string order, string page, string rows, string sort, string GroupId)
        {
            StringBuilder sbGroupIds = new StringBuilder("");
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_MapHeader_Group";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "Id";

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

            string strWhereTemp = "";
            StringBuilder sbPosition = new StringBuilder("");
            ContactChildGroupIds(GroupId, ref sbGroupIds);
            if (sbGroupIds.ToString().EndsWith(","))
            {
                sbGroupIds = new StringBuilder(sbGroupIds.ToString().Substring(0, sbGroupIds.ToString().Length - 1));
            }

            if (GroupId != "")
            {
                if (strWhereTemp != "")
                {
                    strWhereTemp = strWhereTemp + " and GroupId in (" + sbGroupIds.ToString() + ") ";
                }
                else
                {
                    strWhereTemp = strWhereTemp + "  GroupId in (" + sbGroupIds.ToString() + ") ";
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
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\n", "&nbsp;").Replace("\r\n", "&nbsp;")).Replace("\"", "&quot;").Replace("'", "&apos;"));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\n", "&nbsp;").Replace("\r\n", "&nbsp;")).Replace("\"", "&quot;").Replace("'", "&apos;"));
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

            sb = new StringBuilder(sb.ToString().Replace("\\", "/"));

            return sb.ToString();
        }

        public string GetAllData(string page, string rows)
        {
            StringBuilder sbGroupIds = new StringBuilder("");
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter();
            param[0].SqlDbType = SqlDbType.VarChar;
            param[0].ParameterName = "@TableName";
            param[0].Direction = ParameterDirection.Input;
            param[0].Value = "V_MapHeader_Group";

            param[1] = new SqlParameter();
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].ParameterName = "@FieldKey";
            param[1].Direction = ParameterDirection.Input;
            param[1].Value = "Id";

            param[2] = new SqlParameter();
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].ParameterName = "@FieldShow";
            param[2].Direction = ParameterDirection.Input;
            param[2].Value = "*";

            param[3] = new SqlParameter();
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].ParameterName = "@FieldOrder";
            param[3].Direction = ParameterDirection.Input;
            param[3].Value = " GroupName " + " " + " asc ";

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

            string strWhereTemp = "";
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

                string[] strFiledArray = strFileds1.Split(',');
                for (int j = 0; j < strFiledArray.Length; j++)
                {
                    switch (strFiledArray[j])
                    {
                        case "MapDes":
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i]["GroupName"].ToString() + "---" + dt.Rows[i]["MapName"].ToString());
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i]["GroupName"].ToString() + "---" + dt.Rows[i]["MapName"].ToString());
                            }
                            break;
                        default:
                            if (j != strFiledArray.Length - 1)
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\",", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\n", "&nbsp;").Replace("\r\n", "&nbsp;")).Replace("\"", "&quot;").Replace("'", "&apos;"));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\":\"{1}\"", strFiledArray[j], dt.Rows[i][strFiledArray[j]] == DBNull.Value ? "" : (dt.Rows[i][strFiledArray[j]].ToString().Replace("\n", "&nbsp;").Replace("\r\n", "&nbsp;")).Replace("\"", "&quot;").Replace("'", "&apos;"));
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

            sb = new StringBuilder(sb.ToString().Replace("\\", "/"));

            return sb.ToString();
        }

        public void ContactChildGroupIds(string groupId, ref StringBuilder sb)
        {
            string strSQL = "";
            DataSet ds = null;
            DataTable dt = null;
            sb.AppendFormat("{0},", groupId);
            strSQL = "select * from T_Group where GroupUpID=" + groupId;
            ds = SqlServerHelper.Query(strSQL);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ContactChildGroupIds(dt.Rows[i]["ID"].ToString(), ref sb);
                    }
                }
            }
        }

        public string AddMap(FormCollection form)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip1") + "\"}";
            if (!Directory.Exists(Server.MapPath(STR_MAP_FOLDER)))
            {
                Directory.CreateDirectory(Server.MapPath(STR_MAP_FOLDER));
            }
            HttpPostedFileBase mapFile = Request.Files["mapFile_Add"];

            string txtMapName_Add =Request.Form["txtMapName_Add"].ToString();
            string hid_AreaId_MapAdd = Request.Form["hid_AreaId_MapAdd"].ToString();

            string strFileName = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random()).Next(10).ToString("00") + "]";
            string strSourceFileNameWithExtension = mapFile.FileName.Substring(mapFile.FileName.LastIndexOf("\\") + 1);
            string strSourceFileNameWithOutExtension = strSourceFileNameWithExtension.Substring(0, strSourceFileNameWithExtension.LastIndexOf("."));
            string strSourceFileNameExtensionName = strSourceFileNameWithExtension.Substring(strSourceFileNameWithExtension.LastIndexOf(".") + 1);
            string strFullFilePath = Server.MapPath(STR_MAP_FOLDER + strSourceFileNameWithOutExtension + strFileName + "." + strSourceFileNameExtensionName);
            try
            {
                //保存地图文件
                mapFile.SaveAs(strFullFilePath);
                if (new T_MapHeader().Add(Convert.ToInt32(hid_AreaId_MapAdd), txtMapName_Add, "images/map/imgs/" + strSourceFileNameWithOutExtension + strFileName + "." + strSourceFileNameExtensionName))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip2") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip3") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
            }

            return strRet;
        }

        public string UpdateMap(FormCollection form)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip4") + "\"}";
            if (!Directory.Exists(Server.MapPath(STR_MAP_FOLDER)))
            {
                Directory.CreateDirectory(Server.MapPath(STR_MAP_FOLDER));
            }
            HttpPostedFileBase mapFile = Request.Files["mapFile_Update"];

            string txtMapName_Update = Request.Form["txtMapName_Update"].ToString();
            string hid_AreaId_MapUpdate = Request.Form["hid_AreaId_MapUpdate"].ToString();
            string hid_MapId_MapUpdate = Request.Form["hid_MapId_MapUpdate"].ToString();
            string mapFilePath_Old = Request.Form["mapFilePath_Old"].ToString();

            string strFileName = "";
            string strSourceFileNameWithExtension = "";
            string strSourceFileNameWithOutExtension = "";
            string strSourceFileNameExtensionName = "";
            string strFullFilePath = "";
            if (mapFile.ContentLength == 0)
            {
                strFullFilePath = mapFilePath_Old;
                if (new T_MapHeader().Update(Convert.ToInt32(hid_AreaId_MapUpdate), txtMapName_Update, strFullFilePath, Convert.ToInt32(hid_MapId_MapUpdate)))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip5") + "\"}";
                }
            }
            else
            {
                strFileName = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random()).Next(10).ToString("00") + "]";
                strSourceFileNameWithExtension = mapFile.FileName.Substring(mapFile.FileName.LastIndexOf("\\") + 1);
                strSourceFileNameWithOutExtension = strSourceFileNameWithExtension.Substring(0, strSourceFileNameWithExtension.LastIndexOf("."));
                strSourceFileNameExtensionName = strSourceFileNameWithExtension.Substring(strSourceFileNameWithExtension.LastIndexOf(".") + 1);
                strFullFilePath = Server.MapPath(STR_MAP_FOLDER + strSourceFileNameWithOutExtension + strFileName + "." + strSourceFileNameExtensionName);

                try
                {
                    //保存地图文件
                    mapFile.SaveAs(strFullFilePath);
                    if (new T_MapHeader().Update(Convert.ToInt32(hid_AreaId_MapUpdate), txtMapName_Update, "images/map/imgs/" + strSourceFileNameWithOutExtension + strFileName + "." + strSourceFileNameExtensionName, Convert.ToInt32(hid_MapId_MapUpdate)))
                    {
                        strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip5") + "\"}";
                    }
                }
                catch (Exception ex)
                {
                    strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip6") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
                }
            }
            return strRet;
        }

        public string DeleteMap(FormCollection form)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip7") + "\"}";
            string Id = Request.QueryString["Id"].ToString();

            try
            {
                if (new T_MapHeader().Delete(Convert.ToInt32(Id)))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip8") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapDesign_Control_ErrorTip9") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
            }
            return strRet;
        }
    }
}
