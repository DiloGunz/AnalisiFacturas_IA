using ADIA.Model.Domain.Entities;
using ADIA.Repository.Interfaces.Repositories.AnalysisRepositories;
using ADIA.Repository.SqlServer.Generic;
using ADIA.Uow.SqlServer.Core;

namespace ADIA.Repository.SqlServer.Repositories.AnalysisRepositories;

public class AnalysisRepository : GenericRepository<Analysis>, IAnalysisRepository
{
	public AnalysisRepository(AppDbContext context)
	{
		_dbSet = context.Set<Analysis>();
		_context = context;
	}
}