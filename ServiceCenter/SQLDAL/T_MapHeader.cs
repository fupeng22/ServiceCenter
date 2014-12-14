
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtility;
using System.Collections;
using System.Data;

namespace SQLDAL
{
   public class T_MapHeader
    {
        public T_MapHeader()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 添加地图
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="MapName"></param>
        /// <param name="MapPath"></param>
        /// <returns></returns>
        public bool Add(int GroupId, string MapName, string MapPath)
        {
            string strSql = "";
            bool bOK = false;
            strSql = string.Format(@"INSERT INTO [T_MapHeader]
                                       ([GroupId]
                                       ,[MapName]
                                       ,[MapPath])
                                 VALUES
                                       ({0}
                                       ,'{1}'
                                       ,'{2}'
                                       )", GroupId, MapName, MapPath);
            if (SqlServerHelper.ExecuteSql(strSql) > 0)
            {
                bOK = true;
            }
            return bOK;
        }

        /// <summary>
        /// 修改地图
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="MapName"></param>
        /// <param name="MapPath"></param>
        /// <param name="MapId"></param>
        /// <returns></returns>
        public bool Update(int GroupId, string MapName, string MapPath, int MapId)
        {
            string strSql = "";
            bool bOK = false;
            strSql = string.Format(@"UPDATE [T_MapHeader]
                                   SET [GroupId] = {0}
                                      ,[MapName] = '{1}'
                                      ,[MapPath] = '{2}'
                                 WHERE Id={3}", GroupId, MapName, MapPath, MapId);
            if (SqlServerHelper.ExecuteSql(strSql) > 0)
            {
                bOK = true;
            }
            return bOK;
        }

        /// <summary>
        /// 修改地图
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="MapName"></param>
        /// <param name="MapPath"></param>
        /// <param name="MapId"></param>
        /// <returns></returns>
        public bool Delete(int MapId)
        {
            ArrayList sqlArr = new ArrayList();
            bool bOK = false;
            sqlArr.Add(string.Format(@"DELETE  FROM dbo.T_MapPots
                                    FROM    dbo.T_MapPots TMP
                                            INNER JOIN dbo.T_MapHeader TMH ON TMH.Id = TMP.MapId WHERE TMH.Id={0}", MapId));
            sqlArr.Add(string.Format(@"delete from dbo.T_MapHeader WHERE  Id={0}", MapId));
            try
            {
                SqlServerHelper.ExecuteSqlTran(sqlArr);
                bOK = true;
            }
            catch (Exception ex)
            {

            }
            return bOK;
        }

        public DataSet LoadMapInfoByMapId(int MapId)
        {
            DataSet ds = null;
            string strSQL = "";
            strSQL = string.Format(@"select * from V_MapHeader_Group where Id={0}", MapId);
            ds = SqlServerHelper.Query(strSQL);

            return ds;
        }

        public string GetGroupSubIdsByMapId(int MapId)
        {
            string strRet = "";
            DataSet ds = null;
            DataTable dt = null;
            StringBuilder sb = new StringBuilder("");
            string strSQL = "";
            strSQL = "select * from T_MapHeader where Id=" + MapId;
            ds = SqlServerHelper.Query(strSQL);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    GetGroupSubIdsByGroupId(ref sb, Convert.ToInt32(dt.Rows[0]["GroupId"].ToString()));
                }
            }
            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }
            strRet = sb.ToString();
            return strRet;
        }

        public void GetGroupSubIdsByGroupId(ref StringBuilder sb, int GroupId)
        {
            DataSet ds = null;
            DataTable dt = null;
            sb.AppendFormat("{0},", GroupId);
            ds = SqlServerHelper.Query("select * from T_Group where GroupUpID=" + GroupId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetGroupSubIdsByGroupId(ref sb, Convert.ToInt32(dt.Rows[i]["ID"].ToString()));
                    }
                }
            }
        }
    }
}
