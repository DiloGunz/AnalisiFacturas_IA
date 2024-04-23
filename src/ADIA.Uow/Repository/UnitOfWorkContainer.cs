using ADIA.Uow.Interfaces;
using ADIA.Uow.SqlServer.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ADIA.Uow.Repository;

public class UnitOfWorkContainer : IUnitOfWork
{
    public IUnitOfWorkRepository Repository { get; }


    private readonly AppDbContext _context;

    private bool _disposed;

    //public ApplicationDbContext _context { get; }

    public UnitOfWorkContainer(
        AppDbContext context,
        IUnitOfWorkRepository unitOfWorkRepository)
    {
        _context = context;
        Repository = unitOfWorkRepository;
    }

    #region Detect Changes
    public void DetectChanges()
    {
        _context.ChangeTracker.DetectChanges();
    }
    #endregion

    #region Save Changes
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Transactions
    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public void CommitTransaction()
    {
        _context.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        _context.Database.RollbackTransaction();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    public async Task ExecuteTransactionAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }

    #endregion


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }


    public void Dispose()
    {
        //if (_context != null)
        //{
        //    _context.Dispose();
        //}
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    public DbSet<TEntity> GetEntity<TEntity>() where TEntity : class
    {
        return _context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetEntityAsQueryable<TEntity>() where TEntity : class
    {
        return _context.Set<TEntity>().AsQueryable();
    }
}