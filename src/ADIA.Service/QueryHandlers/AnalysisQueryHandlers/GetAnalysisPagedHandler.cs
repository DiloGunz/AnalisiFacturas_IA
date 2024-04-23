using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Model.DataTransfer.Queries;
using ADIA.Shared.Collection;
using ADIA.Uow.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ADIA.Shared.Extensions;

namespace ADIA.Service.QueryHandlers.AnalysisQueryHandlers;

public class GetAnalysisPagedHandler : IRequestHandler<GetAnalysisPagedQuery, DataCollection<AnalysisListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAnalysisListHandler> _logger;
    private readonly IMapper _mapper;

    public GetAnalysisPagedHandler(IUnitOfWork unitOfWork, ILogger<GetAnalysisListHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<DataCollection<AnalysisListDto>> Handle(GetAnalysisPagedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = await _unitOfWork.Repository.Analysis
                .GetPagedAsync(
                    request.Page,
                    request.PageSize,
                    x => x.OrderByDescending(y => y.AnalysisDate),
                    predicate: x => x.AnalysisDate >= request.DtFrom && x.AnalysisDate <= request.DtTo.GetEndDay() && x.FileName.Contains(request.SearchString),
                    include: x => x.Include(y => y.AnalysisResponse));

            return _mapper.Map<DataCollection<AnalysisListDto>>(query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return new DataCollection<AnalysisListDto>();
    }
}