using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Model.DataTransfer.Queries.Generics;
using ADIA.Shared.Collection;
using MediatR;

namespace ADIA.Model.DataTransfer.Queries;

public record GetAnalysisPagedQuery : GenericDateQuery, IRequest<DataCollection<AnalysisListDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}