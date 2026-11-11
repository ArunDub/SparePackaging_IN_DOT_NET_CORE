namespace SparePackaging.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
public sealed class OracleConnectionFactory
{
    private readonly string _connectionString;

    public OracleConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OracleConnection") ?? throw new InvalidOperationException("Oracle connection string is missing.");
    }

    public OracleConnection CreateConnection()
    {
        return new OracleConnection(_connectionString);
    }
}
