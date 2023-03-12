using Wati.Template.Data;
using Wati.Template.Repository.Repositories.Interfaces;

namespace Wati.Template.Repository.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly IDatabaseContext _databaseContext;

    public Repository(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
        
    public async ValueTask<TEntity> FindAsync(Guid id) => await _databaseContext.Set<TEntity>().FindAsync(id);

    public IQueryable<TEntity> All() => _databaseContext.Set<TEntity>();
}