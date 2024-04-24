using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Model.DataTransfer.Queries;
using ADIA.Shared.Collection;
using ADIA.Uow.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ADIA.Shared.Extensions;
using ADIA.Model.Domain.Entities;

namespace ADIA.Service.QueryHandlers.AnalysisQueryHandlers;

public class GetAnalysisPagedHandler : IRequestHandler<GetAnalysisPagedQuery, DataCollection<AnalysisListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAnalysisPagedHandler> _logger;
    private readonly IMapper _mapper;

    public GetAnalysisPagedHandler(IUnitOfWork unitOfWork, ILogger<GetAnalysisPagedHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DataCollection<AnalysisListDto>> Handle(GetAnalysisPagedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var queryResult = await FetchPagedAnalysisAsync(request, cancellationToken);
            return MapQueryResultToDto(queryResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching paged analysis data.");
            throw;  // Rethrow to maintain the stack trace and allow higher layers to handle the error appropriately.
        }
    }

    private async Task<DataCollection<Analysis>> FetchPagedAnalysisAsync(GetAnalysisPagedQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository.Analysis.GetPagedAsync(
            request.Page,
            request.PageSize,
            x => x.OrderByDescending(y => y.AnalysisDate),
            predicate: x => x.AnalysisDate >= request.DtFrom && x.AnalysisDate <= request.DtTo.GetEndDay() && x.FileName.Contains(request.SearchString),
            include: x => x.Include(y => y.AnalysisResponse));
    }

    private DataCollection<AnalysisListDto> MapQueryResultToDto(DataCollection<Analysis> queryResult)
    {
        if (queryResult == null)
            return new DataCollection<AnalysisListDto>();

        var mappedData = _mapper.Map<IEnumerable<AnalysisListDto>>(queryResult.Items);
        return new DataCollection<AnalysisListDto>
        {
            Items = mappedData,
            Total = queryResult.Total,
            Page = queryResult.Page,
            Take = queryResult.Take
        };
    }
}