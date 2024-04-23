using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Model.DataTransfer.Queries.Generics;
using MediatR;

namespace ADIA.Model.DataTransfer.Queries;

public record GetAnalysisListQuery : GenericDateQuery, IRequest<IEnumerable<AnalysisListDto>>
{

}