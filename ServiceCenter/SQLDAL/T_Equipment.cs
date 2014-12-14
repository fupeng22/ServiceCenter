using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace SQLDAL
{
    public class T_Equipment
    {
        public const string strFileds = "DeviceName,ID";

        public bool addEquipment(M_Equipment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [T_Equipment]
                           ([DeviceServiceID]
                           ,[DeviceName]
                           ,[Group_ID])
                     VALUES
                           (@DeviceServiceID
                           ,@DeviceName
                           ,@Group_ID)");

            SqlParameter[] parameters = {
                    new SqlParameter("@DeviceServiceID",SqlDbType.NVarChar),
                    new SqlParameter("@DeviceName",SqlDbType.NVarChar ),
                    new SqlParameter("@Group_ID",SqlDbType.Int)
            };
            parameters[0].Value = model.DeviceServiceID;
            parameters[1].Value = model.DeviceName;
            parameters[2].Value = model.Group_ID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateEquipment(M_Equipment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [T_Equipment]
                                   SET [DeviceServiceID] =@DeviceServiceID
                                      ,[DeviceName] =@DeviceName
                                      ,[Group_ID] = @Group_ID
                         WHERE ID=@ID");

            SqlParameter[] parameters = {
                    new SqlParameter("@DeviceServiceID",SqlDbType.NVarChar),
                    new SqlParameter("@DeviceName",SqlDbType.NVarChar ),
                    new SqlParameter("@Group_ID",SqlDbType.Int),
                    new SqlParameter("@ID",SqlDbType.Int)
            };
            parameters[0].Value = model.DeviceServiceID;
            parameters[1].Value = model.DeviceName;
            parameters[2].Value = model.Group_ID;
            parameters[3].Value = model.ID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool EquipmentNameExist(string DeviceName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [T_Equipment]");
            strSql.Append(" WHERE (DeviceName = '" + DeviceName + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EquipmentNameExist(int ID, string DeviceName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [T_Equipment]");
            strSql.Append(" WHERE (DeviceName = '" + DeviceName + "' and ID<>" + ID + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deleteEquipment(string IDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [T_Equipment] where ID in (" + IDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string LoadAvialableEquipmentByMapId(int MapId)
        {
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = null;
            string strSubGroupIds = "";
            try
            {
                strSubGroupIds = new T_MapHeader().GetGroupSubIdsByMapId(MapId);
                if (strSubGroupIds != null)
                {
                    ds = SqlServerHelper.Query(string.Format(@"SELECT  *
                                                FROM    dbo.T_Equipment
                                                WHERE   Group_ID IN ( {0} )
                                                        AND ID NOT IN ( SELECT  EquipmentId
                                                                        FROM    dbo.T_MapPots
                                                                        WHERE   MapId IN ( SELECT   Id
                                                                                           FROM     dbo.T_MapHeader
                                                                                           WHERE    GroupId IN ( {0} ) ) )
                                                        AND ID NOT IN (
                                                        SELECT  DISTINCT
                                                                TE.ID
                                                        FROM    dbo.T_Equipment TE
                                                                INNER JOIN dbo.T_MapPots TMP ON TE.ID = TMP.EquipmentId
                                                                INNER JOIN dbo.T_MapHeader TMH ON tmh.Id = TMP.MapId )", strSubGroupIds));

                    dt = ds.Tables[0];


                    sb.Append("{");
                    sb.AppendFormat("\"total\":{0}", Convert.ToInt32(dt.Rows.Count));
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
                }

            }
            catch (Exception ex)
            {

            }

            return sb.ToString();
        }

        public string LoadAvialableEquipmentByMapId_Update(int MapId, int EquipmentId)
        {
            StringBuilder sb = new StringBuilder("");
            DataSet ds = null;
            DataTable dt = null;
            string strSubGroupIds = "";
            try
            {
                strSubGroupIds = new T_MapHeader().GetGroupSubIdsByMapId(MapId);
                if (strSubGroupIds != null)
                {
                    ds = SqlServerHelper.Query(string.Format(@"SELECT  [ID] ,
                                                                DeviceServiceID ,
                                                                DeviceName ,
                                                                [Group_ID] ,
                                                                iDeviceChannal ,
                                                                iNvr_ID ,
                                                                dtLastSearchTime ,
                                                                iSearchInterval
                                                        FROM    dbo.T_Equipment
                                                        WHERE   Group_ID IN ( {0} )
                                                                AND ID NOT IN ( SELECT  EquipmentId
                                                                                FROM    dbo.T_MapPots
                                                                                WHERE   MapId IN ( SELECT   Id
                                                                                                   FROM     dbo.T_MapHeader
                                                                                                   WHERE    GroupId IN ( {0} ) ) )
                                                                AND ID NOT IN (
                                                                SELECT  DISTINCT
                                                                        TE.ID
                                                                FROM    dbo.T_Equipment TE
                                                                        INNER JOIN dbo.T_MapPots TMP ON TE.ID = TMP.EquipmentId
                                                                        INNER JOIN dbo.T_MapHeader TMH ON tmh.Id = TMP.MapId )
                                                        UNION
                                                        SELECT  [ID] ,
                                                                DeviceServiceID ,
                                                                DeviceName ,
                                                                [Group_ID] ,
                                                                iDeviceChannal ,
                                                                iNvr_ID ,
                                                                dtLastSearchTime ,
                                                                iSearchInterval
                                                        FROM    dbo.T_Equipment
                                                        WHERE   ID = {1}", strSubGroupIds, EquipmentId));
                    dt = ds.Tables[0];


                    sb.Append("{");
                    sb.AppendFormat("\"total\":{0}", Convert.ToInt32(dt.Rows.Count));
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
                }
            }
            catch (Exception ex)
            {

            }

            return sb.ToString();
        }
    }
}
