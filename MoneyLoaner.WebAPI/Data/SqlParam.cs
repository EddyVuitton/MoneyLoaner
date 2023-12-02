using Microsoft.Data.SqlClient;
using System.Data;

namespace MoneyLoaner.WebAPI.Data;

public static class SqlParam
{
    public static object CreateParameter(string name, object value, SqlDbType type)
    {
        return new SqlParameter()
        {
            ParameterName = name,
            Value = value ?? DBNull.Value,
            SqlDbType = type
        };
    }
}