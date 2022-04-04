using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSEACalculator.OtherRes
{
    public static class ExtensionMethods
    {
        public static SqlParameter AddWithNullValue<T>(this SqlParameterCollection parms, string parameterName, T? nullable) where T : struct
        {
            if (nullable.HasValue)
            {
                return parms.AddWithValue(parameterName, nullable.Value);
            }
            else
            {
                return parms.AddWithValue(parameterName, DBNull.Value); 
            }
        }

    }
}
