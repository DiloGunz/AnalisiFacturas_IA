using System.Linq.Expressions;

namespace ADIA.Repository.Interfaces.Generic;

public interface IQueryableRepository<T> where T : class
{
    IQueryable<T> GetEntity(Expression<Func<T, bool>>? predicate = null);
    IQueryable<T> GetEntityAsNoTracking(Expression<Func<T, bool>>? predicate = null);
}