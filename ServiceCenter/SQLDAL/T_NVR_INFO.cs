using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Model;

namespace SQLDAL
{
    public class T_NVR_INFO
    {
        public bool addNVR(M_NVR_INFO model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [NVR_INFO]
                               ([NVR_NAME]
                               ,[NVR_IP]
                               ,[NVR_PORT]
                               ,[NVR_USER]
                               ,[NVR_PW]
                               ,[NVR_BAK],NVR_DHL_IP)
                         VALUES
                               (@NVR_NAME
                               ,@NVR_IP
                               ,@NVR_PORT
                               ,@NVR_USER
                               ,@NVR_PW
                               ,@NVR_BAK,@NVR_DHL_IP)");

            SqlParameter[] parameters = {
                    new SqlParameter("@NVR_NAME",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_IP",SqlDbType.NVarChar ),
                    new SqlParameter("@NVR_PORT",SqlDbType.Int),
                    new SqlParameter("@NVR_USER",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_PW",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_BAK",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_DHL_IP",SqlDbType.NVarChar)
            };
            parameters[0].Value = model.NVR_Name;
            parameters[1].Value = model.NVR_Ip;
            parameters[2].Value = model.NVR_Port;
            parameters[3].Value = model.NVR_User;
            parameters[4].Value = model.NVR_Pw;
            parameters[5].Value = model.NVR_Bak;
            parameters[6].Value = model.NVR_DHL_IP;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateNVR(M_NVR_INFO model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [NVR_INFO]
                            SET [NVR_NAME] =@NVR_NAME
                                ,[NVR_IP] =@NVR_IP
                                ,[NVR_PORT] =@NVR_PORT
                                ,[NVR_USER] =@NVR_USER
                                ,[NVR_PW] =@NVR_PW
                                ,[NVR_BAK] =@NVR_BAK,NVR_DHL_IP=@NVR_DHL_IP
                            WHERE NVR_ID=@NVR_ID");

            SqlParameter[] parameters = {
                    new SqlParameter("@NVR_NAME",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_IP",SqlDbType.NVarChar ),
                    new SqlParameter("@NVR_PORT",SqlDbType.Int),
                    new SqlParameter("@NVR_USER",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_PW",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_BAK",SqlDbType.NVarChar),
                    new SqlParameter("@NVR_DHL_IP",SqlDbType.Int),
                    new SqlParameter("@NVR_ID",SqlDbType.Int)
            };
            parameters[0].Value = model.NVR_Name;
            parameters[1].Value = model.NVR_Ip;
            parameters[2].Value = model.NVR_Port;
            parameters[3].Value = model.NVR_User;
            parameters[4].Value = model.NVR_Pw;
            parameters[5].Value = model.NVR_Bak;
            parameters[6].Value = model.NVR_DHL_IP;
            parameters[7].Value = model.NVR_Id;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool NVRNameExist(string NVRName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [NVR_INFO]");
            strSql.Append(" WHERE (NVR_NAME = '" + NVRName + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool NVRNameExist(int ID, string NVRName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [NVR_INFO]");
            strSql.Append(" WHERE (NVR_NAME = '" + NVRName + "' and NVR_ID<>" + ID + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deleteNVR(string IDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [NVR_INFO] where NVR_ID in (" + IDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetAllNVR()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM NVR_INFO  order by NVR_NAME");

            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }
    }
}
