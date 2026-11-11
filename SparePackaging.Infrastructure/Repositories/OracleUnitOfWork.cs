using SparePackaging.Domain.Interfaces;
using SparePackaging.Infrastructure.Persistence;

namespace SparePackaging.Infrastructure.UnitOfWork;

public sealed class OracleUnitOfWork : IUnitOfWork
{
    private readonly OracleDbSession _session;

    public IVendorItemRepository VendorItemRepository { get; }

    public OracleUnitOfWork(
        OracleDbSession session,
        IVendorItemRepository vendorItemRepository)
    {
        _session = session;
        VendorItemRepository = vendorItemRepository;
    }

    public Task BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        return _session.BeginTransactionAsync(
            cancellationToken);
    }

    public Task CommitAsync(
        CancellationToken cancellationToken = default)
    {
        return _session.CommitAsync(
            cancellationToken);
    }

    public Task RollbackAsync(
        CancellationToken cancellationToken = default)
    {
        return _session.RollbackAsync(
            cancellationToken);
    }

    public void Dispose()
    {
        // OracleDbSession is disposed by the DI container.
    }
}
