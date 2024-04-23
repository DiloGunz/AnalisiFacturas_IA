using ADIA.Model.Domain.Entities;
using ADIA.Repository.Interfaces.Repositories.AnalysisResponseRepositories;
using ADIA.Repository.SqlServer.Generic;
using ADIA.Uow.SqlServer.Core;

namespace ADIA.Repository.SqlServer.Repositories.AnalysisResponseRepositories;

public class AnalysisResponseRepository : GenericRepository<AnalysisResponse>, IAnalysisResponseRepository
{
	public AnalysisResponseRepository(AppDbContext context)
	{
		_dbSet = context.Set<AnalysisResponse>();
		_context = context;
	}
}