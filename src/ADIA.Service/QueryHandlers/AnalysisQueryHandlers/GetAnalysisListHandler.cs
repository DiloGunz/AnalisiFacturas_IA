using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Model.DataTransfer.Queries;
using ADIA.Uow.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ADIA.Service.QueryHandlers.AnalysisQueryHandlers;

public class GetAnalysisListHandler : IRequestHandler<GetAnalysisListQuery, IEnumerable<AnalysisListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAnalysisListHandler> _logger;
    private readonly IMapper _mapper;

    public GetAnalysisListHandler(IUnitOfWork unitOfWork, ILogger<GetAnalysisListHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<IEnumerable<AnalysisListDto>> Handle(GetAnalysisListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = await _unitOfWork.Repository
                .Analysis
                .GetAllAsNoTrackingAsync(
                    predicate: x => x.AnalysisDate >= request.DtFrom && x.AnalysisDate <= request.DtTo && x.FileName.Contains(request.SearchString),
                    include: x => x.Include(y => y.AnalysisResponse),
                    orderBy: x => x.OrderByDescending(y => y.AnalysisDate));

            return _mapper.Map<List<AnalysisListDto>>(query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return new List<AnalysisListDto>();
    }
}