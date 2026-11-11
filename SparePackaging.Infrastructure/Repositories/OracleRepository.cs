
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace SparePackaging.Infrastructure.Repositories;

public abstract class OracleRepository
{
    protected static OracleCommand CreateCommand( OracleConnection connection, string procedureName,OracleTransaction? transaction = null)
    {
        var command = connection.CreateCommand();

        command.CommandText = procedureName;
        command.CommandType = CommandType.StoredProcedure;
        command.BindByName = true;

        if (transaction != null)
        {
            command.Transaction = transaction;
        }

        return command;
    }

    protected static string GetString(object value)
    {
        return value == DBNull.Value || value == null? string.Empty: Convert.ToString(value) ?? string.Empty;
    }

    protected static int GetInt(object value)
    {
        return int.TryParse(GetString(value),out int result)? result: 0;
    }
}
