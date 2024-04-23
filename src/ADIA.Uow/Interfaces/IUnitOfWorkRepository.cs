using ADIA.Repository.Interfaces.Repositories.AnalysisRepositories;
using ADIA.Repository.Interfaces.Repositories.AnalysisResponseRepositories;

namespace ADIA.Uow.Interfaces;

public interface IUnitOfWorkRepository
{
    IAnalysisRepository Analysis { get; }
    IAnalysisResponseRepository AnalysisResponses { get; }
}