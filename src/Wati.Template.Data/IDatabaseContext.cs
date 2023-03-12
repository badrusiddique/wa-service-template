using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Wati.Template.Data;

public interface IDatabaseContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    EntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;

    void ChangeEntityState(object entity, EntityState state);

    ValueTask<int> CommitChangesAsync();

    ValueTask<int> CommitChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
}