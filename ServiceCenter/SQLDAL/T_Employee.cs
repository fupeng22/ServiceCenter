using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;
using System.Data.SqlClient;

namespace SQLDAL
{
    public class T_Employee
    {
        public bool addEmployee(M_Employee model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [T_Employee]
                               ([EmpNO]
                               ,[EmpName]
                               ,[EmpDepartment])
                         VALUES
                               (@EmpNO
                               ,@EmpName
                               ,@EmpDepartment)");

            SqlParameter[] parameters = {
                    new SqlParameter("@EmpNO",SqlDbType.NVarChar),
                    new SqlParameter("@EmpName",SqlDbType.NVarChar ),
                    new SqlParameter("@EmpDepartment", SqlDbType.Int)
            };
            parameters[0].Value = model.EmpNO;
            parameters[1].Value = model.EmpName;
            parameters[2].Value = model.EmpDepartment;

            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        public bool updateEmployee(M_Employee model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [T_Employee]
                               SET [EmpNO] =@EmpNO
                                  ,[EmpName] =@EmpName
                                  ,[EmpDepartment] =@EmpDepartment
                             WHERE cId=@cId");

            SqlParameter[] parameters = {
                    new SqlParameter("@EmpNO",SqlDbType.NVarChar),
                    new SqlParameter("@EmpName",SqlDbType.NVarChar ),
                    new SqlParameter("@EmpDepartment", SqlDbType.Int),
                    new SqlParameter("@cId",SqlDbType.Int)
            };
            parameters[0].Value = model.EmpNO;
            parameters[1].Value = model.EmpName;
            parameters[2].Value = model.EmpDepartment;
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

        public bool EmployeeNOExist(string EmpNO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [T_Employee]");
            strSql.Append(" WHERE (EmpNO = '" + EmpNO + "')");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EmployeeNOExist(int empId, string EmpNO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *");
            strSql.Append(" FROM [T_Employee]");
            strSql.Append(" WHERE (EmpNO = '" + EmpNO + "' and cId<>" + empId + ")");

            if (DBUtility.SqlServerHelper.Query(strSql.ToString()).Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool deleteEmployee(string cIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [T_Employee] where cId in (" + cIDs + ")");
            if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString()) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetEmp()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM T_Employee order by EmpName");

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

        public DataSet GetEmp(string GroupIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM T_Employee where EmpDepartment in ("+GroupIds+") order by EmpName");

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
