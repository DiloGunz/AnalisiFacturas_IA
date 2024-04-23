using ADIA.Model.Domain.Entities;
using ADIA.Repository.Interfaces.Generic;

namespace ADIA.Repository.Interfaces.Repositories.AnalysisResponseRepositories;

public interface IAnalysisResponseRepository : 
    ICreateRepository<AnalysisResponse>,
    IReadRepository<AnalysisResponse>
{
}