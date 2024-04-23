#region usings

using ADIA.Repository.Interfaces.Repositories.AnalysisRepositories;
using ADIA.Repository.Interfaces.Repositories.AnalysisResponseRepositories;
using ADIA.Repository.SqlServer.Repositories.AnalysisRepositories;
using ADIA.Repository.SqlServer.Repositories.AnalysisResponseRepositories;
using ADIA.Uow.Interfaces;
using ADIA.Uow.SqlServer.Core;

#endregion

namespace ADIA.Uow.Repository;

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
    public UnitOfWorkRepository(AppDbContext context)
    {
        Analysis = new AnalysisRepository(context);
        AnalysisResponses = new AnalysisResponseRepository(context);
    }

    public IAnalysisRepository Analysis { get; }
    public IAnalysisResponseRepository AnalysisResponses { get; }
}