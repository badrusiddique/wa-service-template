namespace Wati.Template.Repository.Repositories.Interfaces;

public interface ICacheDataContext
{
    ValueTask<TEntity> GetByKeyAsync<TEntity>(string key) where TEntity : class;
}