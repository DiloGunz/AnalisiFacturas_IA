using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ADIA.Uow.Interfaces;

public interface IUnitOfWork : IDisposable
{
    void DetectChanges();
    void SaveChanges();
    Task SaveChangesAsync();
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
    void CommitTransaction();
    void RollbackTransaction();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

    IExecutionStrategy CreateExecutionStrategy();

    Task ExecuteTransactionAsync(Func<Task> action);

    DbSet<TEntity> GetEntity<TEntity>() where TEntity : class;
    IQueryable<TEntity> GetEntityAsQueryable<TEntity>() where TEntity : class;


    IUnitOfWorkRepository Repository { get; }

}