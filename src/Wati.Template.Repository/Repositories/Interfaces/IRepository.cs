using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wati.Template.Repository.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    ValueTask<TEntity> FindAsync(Guid id);
    
    IQueryable<TEntity> All();
}