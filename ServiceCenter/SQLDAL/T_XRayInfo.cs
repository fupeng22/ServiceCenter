using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_XRayInfo
    {
        public bool addXRayInfo(M_XRayInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [XRAY_INFO]
                               ([XRAY_NVR_ID]
                               ,[XRAY_TYPE]
                               ,[BEGIN_X]
                               ,[BEGIN_Y]
                               ,[END_Y]
                               ,[RUN_DIRECTOR]
                               ,[CRITICAL_VALUE]
                               ,[USE_FLAG]
                               ,[XRAY_CHANNEL_NO]
                               ,[XRAY_NAME]
                               ,[BeginCheckTime]
                               ,[LastCheckTime]
                               ,[EVERY_INTERNAL])
                         VALUES
                               (@XRAY_NVR_ID
                               ,@XRAY_TYPE
                               ,@BEGIN_X
                               ,@BEGIN_Y
                               ,@END_Y
                               ,@RUN_DIRECTOR
                               ,@CRITICAL_VALUE
                               ,@USE_FLAG
                               ,@XRAY_CHANNEL_NO
                               ,@XRAY_NAME
                               ,@BeginCheckTime
                               ,@LastCheckTime
                               ,@EVERY_INTERNAL)");

            SqlParameter[] parameters = {
                    new SqlParameter("@XRAY_NVR_ID",SqlDbType.Int),
                    new SqlParameter("@XRAY_TYPE",SqlDbType.Int ),
                    new SqlParameter("@BEGIN_X", SqlDbType.Int),
                    new SqlParameter("@BEGIN_Y", SqlDbType.Int),
                    new SqlParameter("@END_Y", SqlDbType.Int),
                    new SqlParameter("@RUN_DIRECTOR", SqlDbType.Int),
                    new SqlParameter("@CRITICAL_VALUE", SqlDbType.Int),
                    new SqlParameter("@USE_FLAG", SqlDbType.Int),
                    new SqlParameter("@XRAY_CHANNEL_NO", SqlDbType.Int),
                    new SqlParameter("@XRAY_NAME", SqlDbType.NVarChar),
                    new SqlParameter("@BeginCheckTime", SqlDbType.DateTime),
                    new SqlParameter("@LastCheckTime", SqlDbType.DateTime),
                    new SqlParameter("@EVERY_INTERNAL", SqlDbType.Int)
            };
            parameters[0].Value = model.XRAY_NVR_ID;
            parameters[1].Value = model.XRAY_TYPE;
            parameters[2].Value = model.BEGIN_X;
            parameters[3].Value = model.BEGIN_Y;
            parameters[4].Value = model.END_Y;
            parameters[5].Value = model.RUN_DIRECTOR;
            parameters[6].Value = model.CRITICAL_VALUE;
            parameters[7].Value = model.USE_FLAG;
            parameters[8].Value = model.XRAY_CHANNEL_NO;
            parameters[9].Value = model.XRAY_NAME;
            parameters[10].Value = model.BeginCheckTime;
            parameters[11].Value = model.LastCheckTime;
            parameters[12].Value = model.EVERY_INTERNAL;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateXRayInfo(M_XRayInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [XRAY_INFO]
                               SET [XRAY_NVR_ID] =@XRAY_NVR_ID
                                  ,[XRAY_TYPE] =@XRAY_TYPE
                                  ,[BEGIN_X] =@BEGIN_X
                                  ,[BEGIN_Y] =@BEGIN_Y
                                  ,[END_Y] =@END_Y
                                  ,[RUN_DIRECTOR] =@RUN_DIRECTOR
                                  ,[CRITICAL_VALUE] =@CRITICAL_VALUE
                                  ,[USE_FLAG] =@USE_FLAG
                                  ,[XRAY_CHANNEL_NO] =@XRAY_CHANNEL_NO
                                  ,[XRAY_NAME] =@XRAY_NAME
                                  ,[BeginCheckTime] =@BeginCheckTime
                                  ,[LastCheckTime] =@LastCheckTime
                                  ,[EVERY_INTERNAL] =@EVERY_INTERNAL
                             WHERE XRAY_ID=@XRAY_ID");

            SqlParameter[] parameters = {
                    new SqlParameter("@XRAY_NVR_ID",SqlDbType.Int),
                    new SqlParameter("@XRAY_TYPE",SqlDbType.Int ),
                    new SqlParameter("@BEGIN_X", SqlDbType.Int),
                    new SqlParameter("@BEGIN_Y", SqlDbType.Int),
                    new SqlParameter("@END_Y", SqlDbType.Int),
                    new SqlParameter("@RUN_DIRECTOR", SqlDbType.Int),
                    new SqlParameter("@CRITICAL_VALUE", SqlDbType.Int),
                    new SqlParameter("@USE_FLAG", SqlDbType.Int),
                    new SqlParameter("@XRAY_CHANNEL_NO", SqlDbType.Int),
                    new SqlParameter("@XRAY_NAME", SqlDbType.NVarChar),
                    new SqlParameter("@BeginCheckTime", SqlDbType.DateTime),
                    new SqlParameter("@LastCheckTime", SqlDbType.DateTime),
                    new SqlParameter("@EVERY_INTERNAL", SqlDbType.Int),
                    new SqlParameter("@XRAY_ID", SqlDbType.Int)
            };
            parameters[0].Value = model.XRAY_NVR_ID;
            parameters[1].Value = model.XRAY_TYPE;
            parameters[2].Value = model.BEGIN_X;
            parameters[3].Value = model.BEGIN_Y;
            parameters[4].Value = model.END_Y;
            parameters[5].Value = model.RUN_DIRECTOR;
            parameters[6].Value = model.CRITICAL_VALUE;
            parameters[7].Value = model.USE_FLAG;
            parameters[8].Value = model.XRAY_CHANNEL_NO;
            parameters[9].Value = model.XRAY_NAME;
            parameters[10].Value = model.BeginCheckTime;
            parameters[11].Value = model.LastCheckTime;
            parameters[12].Value = model.EVERY_INTERNAL;
            parameters[13].Value = model.XRAY_ID;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool XRayNameExist(string XRayName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [XRAY_INFO]");
            strSql.Append(" WHERE (XRAY_NAME = '" + XRayName + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool XRayNameExist(int XRayId, string XRayName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [XRAY_INFO]");
            strSql.Append(" WHERE (XRAY_NAME = '" + XRayName + "' and XRAY_ID<>" + XRayId + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deleteXRayInfo(string cIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [XRAY_INFO] where XRAY_ID in (" + cIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
