using Oracle.ManagedDataAccess.Client;
using SparePackaging.Domain.Interfaces;
using SparePackaging.Infrastructure.Persistence;
using SparePackaging.Infrastructure.UnitOfWork;
using System.Data;
namespace SparePackaging.Infrastructure.Repositories;
public abstract class Repository<TModel> : OracleRepository, IRepository<TModel> where TModel : class
{
    protected readonly OracleDbSession _session;

    protected Repository(OracleDbSession session)
    {
        _session = session;
    }


    // Existing Oracle Procedure Names
    protected abstract string GetAllProcedure { get; }
    protected abstract string GetByIdProcedure { get; }
    protected abstract string CreateProcedure { get; }
    protected abstract string UpdateProcedure { get; }
    protected abstract string DeleteProcedure { get; }

    // Specific Repository will supply parameters
    protected abstract void BindGetAll(OracleCommand cmd);
    protected abstract void BindGetById(OracleCommand cmd, object id);

    protected abstract void BindCreate(OracleCommand cmd, TModel model);

    protected abstract void BindUpdate(OracleCommand cmd, TModel model);

    protected abstract void BindDelete(OracleCommand cmd, TModel model);

    // Read Oracle RefCursor
    protected abstract TModel Map(OracleDataReader reader);

    // Validate existing procedure output parameters
    protected abstract void ValidateWriteResult(OracleCommand cmd);

    // GET ALL

    public async Task<List<TModel>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var result = new List<TModel>();

        var con = await _session
            .GetOpenConnectionAsync(cancellationToken);

        using var cmd = CreateCommand(
            con,
            GetAllProcedure,
            _session.Transaction);

        BindGetAll(cmd);

        using var reader =
            await cmd.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(Map(reader));
        }

        return result;
    }


    // GET BY ID


    public async Task<TModel?> GetByIdAsync(
        object id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var con = await _session
            .GetOpenConnectionAsync(cancellationToken);

        using var cmd = CreateCommand(
            con,
            GetByIdProcedure,
            _session.Transaction);

        BindGetById(cmd, id);

        using var reader =
            await cmd.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return Map(reader);
        }

        return null;
    }



    // COMMON WRITE METHOD


    protected async Task ExecuteWriteAsync(
        string procedureName,
        Action<OracleCommand> bind,
        CancellationToken cancellationToken)
    {
        var transaction = _session.Transaction
            ?? throw new InvalidOperationException(
                "An active transaction is required.");

        var con = await _session
            .GetOpenConnectionAsync(cancellationToken);

        using var cmd = CreateCommand(
            con,
            procedureName,
            transaction);

        bind(cmd);

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        ValidateWriteResult(cmd);
    }

    // CREATE
    public Task CreateAsync(TModel model,CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        return ExecuteSingleAsync(CreateProcedure,cmd => BindCreate(cmd, model),cancellationToken);
    }

    // UPDATE
    public Task UpdateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        return ExecuteSingleAsync(UpdateProcedure, cmd => BindUpdate(cmd, model),cancellationToken);
    }

    // DELETE
    public Task DeleteAsync(TModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        return ExecuteSingleAsync(DeleteProcedure, cmd => BindDelete(cmd, model),cancellationToken);
    }



    private async Task ExecuteSingleAsync(
        string procedureName,
        Action<OracleCommand> bind,
        CancellationToken cancellationToken)
    {
        var con = await _session
            .GetOpenConnectionAsync(cancellationToken);

        var transaction = _session.Transaction
            ?? throw new InvalidOperationException(
                "An active transaction is required.");

        using var cmd = CreateCommand(
            con,
            procedureName,
            transaction);

        bind(cmd);

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        ValidateWriteResult(cmd);
    }


    // CREATE RANGE
    public Task CreateRangeAsync(List<TModel> models,CancellationToken cancellationToken = default)
    {
        return ExecuteRangeAsync(models, CreateProcedure, BindCreate,cancellationToken);
    }

    // UPDATE RANGE
    public Task UpdateRangeAsync(List<TModel> models,CancellationToken cancellationToken = default)
    {
        return ExecuteRangeAsync(models, UpdateProcedure, BindUpdate,cancellationToken);
    }

    // DELETE RANGE
    public Task DeleteRangeAsync(List<TModel> models,CancellationToken cancellationToken = default)
    {
        return ExecuteRangeAsync( models, DeleteProcedure, BindDelete, cancellationToken);
    }


    private async Task ExecuteRangeAsync(
        List<TModel> models,
        string procedureName,
        Action<OracleCommand, TModel> bind,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(models);

        if (models.Count == 0)
            return;

        // Active Transaction Check
        if (_session.Transaction == null)
        {
            throw new InvalidOperationException(
                "An active transaction is required.");
        }

        // Execute all records in the same transaction
        foreach (var model in models)
        {
            ArgumentNullException.ThrowIfNull(model);

            await ExecuteWriteAsync(
                procedureName,
                cmd => bind(cmd, model),
                cancellationToken);
        }
    }

}
