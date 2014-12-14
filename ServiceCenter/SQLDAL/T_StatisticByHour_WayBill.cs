using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SQLDAL
{
    class T_StatisticByHour_WayBill
    {

//        public bool addUser(Model.M_User model)
//        {
//            StringBuilder strSql = new StringBuilder();
//            strSql.Append(@"INSERT INTO [Better_User]
//                               ([urNum]
//                               ,[urPSW]
//                               ,[urName]
//                               ,[urSex]
//                               ,[urAge]
//                               ,[urStaffNum]
//                               ,[urDept]
//                               ,[urDuty]
//                               ,[urUnitCode]
//                               ,[urMemo])
//                         VALUES
//                               (@urNum
//                               ,@urPSW
//                               ,@urName
//                               ,@urSex
//                               ,@urAge
//                               ,@urStaffNum
//                               ,@urDept
//                               ,@urDuty
//                               ,@urUnitCode
//                               ,@urMem)");

//            SqlParameter[] parameters = {
//                    new SqlParameter("@urNum",SqlDbType.NVarChar),
//                    new SqlParameter("@urPSW",SqlDbType.NVarChar ),
//                    new SqlParameter("@urName", SqlDbType.NVarChar),
//                    new SqlParameter("@urSex",SqlDbType.Int),
//                    new SqlParameter("@urAge",SqlDbType.Int),
//                    new SqlParameter("@urStaffNum",SqlDbType.NVarChar),
//                    new SqlParameter("@urDept",SqlDbType.NVarChar),
//                    new SqlParameter("@urDuty",SqlDbType.NVarChar),
//                    new SqlParameter("@urUnitCode",SqlDbType.NVarChar),
//                    new SqlParameter("@urMem",SqlDbType.NVarChar)
//            };
//            parameters[0].Value = model.urNum;
//            parameters[1].Value = model.urPSW;
//            parameters[2].Value = model.urName;
//            parameters[3].Value = model.urSex;
//            parameters[4].Value = model.urAge;
//            parameters[5].Value = model.urStaffNum;
//            parameters[6].Value = model.urDept;
//            parameters[7].Value = model.urDuty;
//            parameters[8].Value = model.urUnitCode;
//            parameters[9].Value = model.urMemo;

//            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
//            {
//                return true;
//            }
//            else
//            {
//                return false;

//            }

//        }

        //public bool UserExists(string userNum)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(" SELECT *");
        //    strSql.Append(" FROM [Better_User]");
        //    strSql.Append(" WHERE (urNum = '" + userNum + "')");

        //    if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool UserExists(int userID, string userNum)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append(" SELECT *");
        //    strSql.Append(" FROM [Better_User]");
        //    strSql.Append(" WHERE (urNum = '" + userNum + "' and urID<>" + userID + ")");

        //    if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public DataSet Get(string urID)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("SELECT * FROM [Better_User] where urID=" + urID);

        //    DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        return ds;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
