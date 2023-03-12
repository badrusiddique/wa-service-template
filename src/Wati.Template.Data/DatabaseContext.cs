using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wati.Template.Data.Entities;

namespace Wati.Template.Data;

public sealed class DatabaseContext : DbContext, IDatabaseContext
{
    #region Private Variables

    private readonly ILogger<DatabaseContext> _logger;

    #endregion

    #region Constructor

    public DatabaseContext(DbContextOptions options, ILogger<DatabaseContext> logger) : base(options)
    {
        _logger = logger;
    }

    #endregion

    #region DbSets

    public DbSet<Domain> Domains { get; set; }

    #endregion

    #region Public Methods

    public void ChangeEntityState(object entity, EntityState state)
    {
        Entry(entity).State = state;
    }

    public new EntityEntry Entry<TEntity>(TEntity entity) where TEntity : class => base.Entry(entity);

    public async ValueTask<int> CommitChangesAsync() => await CommitChangesAsync(true, default);

    public async ValueTask<int> CommitChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
    {
        try
        {
            var result = await SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return result;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"An error is encountered while saving to the database: {ex.Message}", JsonConvert.SerializeObject(ex));
            throw;
        }
    }

    #endregion

    #region Protected Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }

    #endregion
}