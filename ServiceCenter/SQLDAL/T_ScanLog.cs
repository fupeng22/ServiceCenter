using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SQLDAL
{
    public class T_ScanLog
    {


        public DataSet GetAllStations()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT distinct wbOperator FROM T_ScanLog");

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
