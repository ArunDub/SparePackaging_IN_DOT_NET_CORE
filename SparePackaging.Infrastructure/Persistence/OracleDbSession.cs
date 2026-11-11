using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace SparePackaging.Infrastructure.Persistence;
public sealed class OracleDbSession : IDisposable
{
    private readonly OracleConnection _connection;
    private OracleTransaction? _transaction;
    private bool _disposed;

    public OracleDbSession(
        OracleConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.CreateConnection();
    }

    public OracleTransaction? Transaction => _transaction;

    public async Task<OracleConnection> GetOpenConnectionAsync(
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync(cancellationToken);
        }

        return _connection;
    }

    public async Task BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_transaction != null)
        {
            throw new InvalidOperationException(
                "Transaction already started.");
        }

        await GetOpenConnectionAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        _transaction = _connection.BeginTransaction();
    }

    public async Task CommitAsync(
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_transaction == null)
        {
            throw new InvalidOperationException(
                "No active transaction.");
        }

        await _transaction.CommitAsync(cancellationToken);

        _transaction.Dispose();
        _transaction = null;
    }

    public async Task RollbackAsync(
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(cancellationToken);

        _transaction.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;

            _connection.Dispose();
            _disposed = true;
        }
    }
}
