using ADIA.Model.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace ADIA.Uow.SqlServer.Core;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :
        base(options)
    {

    }

    public IDbConnection Connection => Database.GetDbConnection();


    #region DbSets
    // establecer DBSET para que funcione ApplyGlobalFilters
    //public DbSet<AnalysisResponse> AnalysisResponse { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        ModelConfig(builder);
    }

    public override int SaveChanges()
    {
        //ChangeTracker.SetId();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await ChangeTracker.SetIdAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }



    private void ModelConfig(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
        );

    }
}