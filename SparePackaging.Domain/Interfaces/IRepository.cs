
namespace SparePackaging.Domain.Interfaces;

public interface IRepository<TModel> where TModel : class
{
    //Task<List<TModel>> GetAllAsync(CancellationToken cancellationToken = default);

    //Task<TModel?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<List<TModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TModel?> GetByIdAsync(object id,CancellationToken cancellationToken = default);

    Task CreateAsync(TModel model,CancellationToken cancellationToken = default);

    Task UpdateAsync(TModel model,CancellationToken cancellationToken = default);

    Task DeleteAsync(TModel model,CancellationToken cancellationToken = default);

    Task CreateRangeAsync(List<TModel> models,CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(List<TModel> models,CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(List<TModel> models,CancellationToken cancellationToken = default);
}


