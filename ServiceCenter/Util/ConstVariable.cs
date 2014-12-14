using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Util
{
   public static class ConstVariable
    {
       public static string[] USERNAMEARRAY_ADMIN = ConfigurationManager.AppSettings["UserName_Admin"].ToString().ToLower().Trim().Replace("，",",").Split(',');
    }
}
