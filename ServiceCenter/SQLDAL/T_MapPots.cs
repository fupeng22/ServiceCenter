using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtility;
using System.Data;

namespace SQLDAL
{
   public  class T_MapPots
    {
        public T_MapPots()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 添加地图设备点
        /// </summary>
        /// <param name="MapId"></param>
        /// <param name="EquipmentId"></param>
        /// <param name="MapPotName"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        public bool Add(int MapId, int EquipmentId, string MapPotName, double posX, double posY, int Width, int Height)
        {
            string strSql = "";
            bool bOK = false;
            strSql = string.Format(@"INSERT INTO [T_MapPots]
                                    ([MapId]
                                       ,[EquipmentId]
                                       ,[MapPotName]
                                       ,[posX]
                                       ,[posY]
                                        ,[Width]
                                        ,[Height]
                                        )
                                 VALUES
                                       ({0}
                                       ,{1}
                                       ,'{2}'
                                       ,{3}
                                       ,{4}
                                        ,{5},
                                        {6})", MapId, EquipmentId, MapPotName, posX, posY, Width, Height);
            if (SqlServerHelper.ExecuteSql(strSql) > 0)
            {
                bOK = true;
            }
            return bOK;
        }

        /// <summary>
        /// 修改地图设备点
        /// </summary>
        /// <param name="MapId"></param>
        /// <param name="EquipmentId"></param>
        /// <param name="MapPotName"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="MapPotId"></param>
        /// <returns></returns>
        public bool Update(int MapId, int EquipmentId, string MapPotName, double posX, double posY, int Width, int Height, int MapPotId)
        {
            string strSql = "";
            bool bOK = false;
            strSql = string.Format(@"UPDATE [T_MapPots]
                                   SET [MapId] = {0}
                                      ,[EquipmentId] = {1}
                                      ,[MapPotName] = '{2}'
                                      ,[posX] = {3}
                                      ,[posY] = {4},[Width]={5},[Height]={6}
                                 WHERE  Id={7}", MapId, EquipmentId, MapPotName, posX, posY, Width, Height, MapPotId);
            if (SqlServerHelper.ExecuteSql(strSql) > 0)
            {
                bOK = true;
            }
            return bOK;
        }

        public bool UpdatePostion(double posX, double posY, int MapPotId)
        {
            string strSql = "";
            bool bOK = false;
            strSql = string.Format(@"UPDATE [T_MapPots]
                                   SET [posX] = {0}
                                      ,[posY] = {1}
                                 WHERE  Id={2}", posX, posY, MapPotId);
            if (SqlServerHelper.ExecuteSql(strSql) > 0)
            {
                bOK = true;
            }
            return bOK;
        }

        public bool UpdateSize(int Width, int Height, int MapPotId)
        {
            string strSql = "";
            bool bOK = false;
            strSql = string.Format(@"UPDATE [T_MapPots]
                                   SET [Width] = {0}
                                      ,[Height] = {1}
                                 WHERE  Id={2}", Width, Height, MapPotId);
            if (SqlServerHelper.ExecuteSql(strSql) > 0)
            {
                bOK = true;
            }
            return bOK;
        }

        /// <summary>
        /// 删除地图设备点
        /// </summary>
        /// <param name="MapPotId"></param>
        /// <returns></returns>
        public bool Dele(int MapPotId)
        {
            string strSql = "";
            bool bOK = false;
            strSql = string.Format(@"DELETE FROM dbo.T_MapPots WHERE Id={0}", MapPotId);
            if (SqlServerHelper.ExecuteSql(strSql) > 0)
            {
                bOK = true;
            }
            return bOK;
        }

        public string LoadMapPotDetailByMapId(int MapId)
        {
            string strRet = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = null;
            string strSQL = string.Format(@"select * from V_MapPot_MapHeder_Equipment where MapId={0}", MapId);
            sb.Append("[");
            ds = SqlServerHelper.Query(strSQL);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"Id\":\"{0}\",\"MapId\":\"{1}\",\"EquipmentId\":\"{2}\",\"MapPotName\":\"{3}\",\"posX\":\"{4}\",\"posY\":\"{5}\",\"Width\":\"{6}\",\"Height\":\"{7}\",\"EquipmentName\":\"{8}\"", dt.Rows[i]["Id"].ToString(), dt.Rows[i]["MapId"].ToString(), dt.Rows[i]["EquipmentId"].ToString(), dt.Rows[i]["MapPotName"].ToString(), dt.Rows[i]["posX"].ToString(), dt.Rows[i]["posY"].ToString(), dt.Rows[i]["Width"].ToString(), dt.Rows[i]["Height"].ToString(), dt.Rows[i]["DeviceName"].ToString());
                        sb.Append("}");
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }
                }
            }
            sb.Append("]");
            strRet = sb.ToString();
            return strRet;
        }

        public string LoadMapPotDetailByPotId(int MapPotId)
        {
            string strRet = "";
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = null;
            string strSQL = string.Format(@"select * from V_MapPot_MapHeder_Equipment where Id={0}", MapPotId);
            sb.Append("[");
            ds = SqlServerHelper.Query(strSQL);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.Append("{");
                        sb.AppendFormat("\"Id\":\"{0}\",\"MapId\":\"{1}\",\"EquipmentId\":\"{2}\",\"MapPotName\":\"{3}\",\"posX\":\"{4}\",\"posY\":\"{5}\",\"Width\":\"{6}\",\"Height\":\"{7}\",\"DeviceName\":\"{8}\"", dt.Rows[i]["Id"].ToString(), dt.Rows[i]["MapId"].ToString(), dt.Rows[i]["EquipmentId"].ToString(), dt.Rows[i]["MapPotName"].ToString(), dt.Rows[i]["posX"].ToString(), dt.Rows[i]["posY"].ToString(), dt.Rows[i]["Width"].ToString(), dt.Rows[i]["Height"].ToString(), dt.Rows[i]["DeviceName"].ToString());
                        sb.Append("}");
                        if (i != dt.Rows.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }
                }
            }
            sb.Append("]");
            strRet = sb.ToString();
            return strRet;
        }


        /// <summary>
        /// 获取指定地图的所有报警的设备
        /// </summary>
        /// <param name="MapId"></param>
        /// <returns></returns>
        public DataSet GetMapIdByEquipmentId(int EquipmentId)
        {
            DataSet ds = null;
            string strSQL = "";
            strSQL = string.Format(@"select * from T_MapPots where EquipmentId={0}", EquipmentId);
            ds = SqlServerHelper.Query(strSQL);

            return ds;
        }

        public DataSet GetMapPotInfo(int MapPotId)
        {
            DataSet ds = null;
            string strSQL = "";
            strSQL = string.Format(@"select * from V_MapPot_MapHeder_Equipment where Id={0}", MapPotId);
            ds = SqlServerHelper.Query(strSQL);

            return ds;
        }
    }
}
