using ADIA.Model.Domain.Entities;
using ADIA.Repository.Interfaces.Generic;

namespace ADIA.Repository.Interfaces.Repositories.AnalysisRepositories
{
    public interface IAnalysisRepository :
        ICreateRepository<Analysis>, 
        IReadRepository<Analysis>,
        IPagedRepository<Analysis>
    {
    }
}
