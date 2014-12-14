using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace SQLDAL
{
    public class T_Users
    {
        public bool addUser(M_Users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [Users]
                           ([Username]
                           ,[Password]
                           ,[Group_ID])
                     VALUES
                           (@Username
                           ,@Password
                           ,@Group_ID)");

            SqlParameter[] parameters = {
                    new SqlParameter("@Username",SqlDbType.NVarChar),
                    new SqlParameter("@Password",SqlDbType.NVarChar ),
                    new SqlParameter("@Group_ID", SqlDbType.Int)
            };
            parameters[0].Value = model.Username;
            parameters[1].Value = model.Password;
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

        public bool updateUser(M_Users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [Users]
                           SET [Username] =@Username
                              ,[Password] =@Password
                              ,[Group_ID] =@Group_ID
                         WHERE cId=@cId");

            SqlParameter[] parameters = {
                    new SqlParameter("@Username",SqlDbType.NVarChar),
                    new SqlParameter("@Password",SqlDbType.NVarChar ),
                    new SqlParameter("@Group_ID", SqlDbType.Int),
                    new SqlParameter("@cId",SqlDbType.Int)
            };
            parameters[0].Value = model.Username;
            parameters[1].Value = model.Password;
            parameters[2].Value = model.Group_ID;
            parameters[3].Value = model.cId;
          
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool UserExists(string userNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Users]");
            strSql.Append(" WHERE (Username = '" + userNum + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UserExists(int userID, string userNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Users]");
            strSql.Append(" WHERE (Username = '" + userNum + "' and cId<>" + userID + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deleteUsers(string cIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [Users] where cId in (" + cIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Login(string userNum,string userPwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [Users]");
            strSql.Append(" WHERE (Username = '" + userNum + "' and Password='" + userPwd + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetUseByUsername(string strUserName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM V_Users where Username='" + strUserName + "'");

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
