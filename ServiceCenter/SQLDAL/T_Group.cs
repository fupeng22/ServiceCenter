using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;
using System.Data.SqlClient;

namespace SQLDAL
{
    public class T_Group
    {
        public DataSet GetAllTopGroup()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM T_Group where GroupUpID=-1 order by GroupName");

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

        public DataSet GetGroupByGroupId(object groupId)
        {
            StringBuilder strSql = new StringBuilder();
            string strGroupId = "";
            if (groupId == null)
            {
                strGroupId = "-99";
            }
            else
            {
                strGroupId = groupId.ToString();
            }
            strSql.Append("SELECT  * FROM T_Group where ID="+strGroupId+" order by GroupName");

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

        public DataSet GetGroupByUpGroupId(string strUpGroupId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  * FROM T_Group where GroupUpID=" + strUpGroupId + " order by GroupName");

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

        public Boolean TestExistGroupName(string GroupName)
        {
            Boolean bExist = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from T_Group where GroupName='{0}'", GroupName);
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds != null)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    bExist = true;
                }
            }
            return bExist;
        }

        public Boolean TestExistGroupName(string GroupName, string GroupId)
        {
            Boolean bExist = false;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from T_Group where GroupName='{0}' and ID<>{1}", GroupName, GroupId);
            DataSet ds = DBUtility.SqlServerHelper.Query(strSql.ToString());
            if (ds != null)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    bExist = true;
                }
            }
            return bExist;
        }

        public Boolean InsertGroup(M_T_Group m_Group)
        {
            Boolean bOk = false;

            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("insert into T_Group");
                strSql.Append(" (GroupName,GroupUpID)");
                strSql.Append(" values (");
                strSql.Append("@GroupName,@GroupUpID)");

                SqlParameter[] parameters = {
                    new SqlParameter("@GroupName",SqlDbType.NVarChar),
                    new SqlParameter("@GroupUpID", SqlDbType.Int)
                                                    };
                parameters[0].Value = m_Group.GroupName;
                parameters[1].Value = m_Group.GroupUpID;

                if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
                {
                    bOk = true;
                }
                else
                {
                    bOk = false;
                }
            }
            catch (Exception ex)
            {
                bOk = false;
            }

            return bOk;
        }

        /// <summary>
        /// 根据部门ID修改部门信息（特指部门名称）
        /// </summary>
        /// <param name="m_Department"></param>
        /// <returns></returns>
        public Boolean UpdateGroup(M_T_Group m_Group)
        {
            Boolean bOk = false;

            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("update T_Group set ");
                strSql.Append(" GroupName=@GroupName ");
                strSql.Append(" where ID=@ID ");

                SqlParameter[] parameters = {
                    new SqlParameter("@GroupName",SqlDbType.NVarChar),
                    new SqlParameter("@ID", SqlDbType.Int )
                                                    };
                parameters[0].Value = m_Group.GroupName;
                parameters[1].Value = m_Group.ID;

                if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
                {
                    bOk = true;
                }
                else
                {
                    bOk = false;
                }
            }
            catch (Exception ex)
            {
                bOk = false;
            }

            return bOk;
        }

        public Boolean DeleGroup(M_T_Group m_Group)
        {
            Boolean bOk = false;

            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("delete T_Group ");
                strSql.Append(" where ID=@ID ");

                SqlParameter[] parameters = {
                    new SqlParameter("@ID",SqlDbType.Int)
                                                    };
                parameters[0].Value = m_Group.ID;

                if (DBUtility.SqlServerHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
                {
                    bOk = true;
                }
                else
                {
                    bOk = false;
                }
            }
            catch (Exception ex)
            {
                bOk = false;
            }

            return bOk;
        }

        public string GetSubDepartIdsByDepartId(object DepartId_tmp)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = null;
            DataTable dt = null;
            string DepartId = "";

            if (DepartId_tmp==null)
            {
                return "-99";
            }
            DepartId=DepartId_tmp.ToString();

            sb.Append(DepartId + ",");
            ds = GetGroupByUpGroupId(DepartId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetSubDepartIds(ref sb, dt.Rows[i]["ID"].ToString());
                    }
                }
            }

            if (sb.ToString().EndsWith(","))
            {
                sb = new StringBuilder(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }

            return sb.ToString();
        }

        public void GetSubDepartIds(ref StringBuilder sb, string strParentDepartId)
        {
            DataSet ds = null;
            DataTable dt = null;
            sb.Append(strParentDepartId + ",");
            ds = GetGroupByUpGroupId(strParentDepartId);
            if (ds != null)
            {
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetSubDepartIds(ref sb, dt.Rows[i]["ID"].ToString());
                    }
                }
            }
        }
    }
}
