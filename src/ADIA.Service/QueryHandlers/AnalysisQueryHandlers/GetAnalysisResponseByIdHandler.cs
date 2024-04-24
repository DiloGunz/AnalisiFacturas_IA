using ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;
using ADIA.Model.DataTransfer.Queries;
using ADIA.Uow.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ADIA.Service.QueryHandlers.AnalysisQueryHandlers;

public class GetAnalysisResponseByIdHandler : IRequestHandler<GetAnalysisResponseByIdQuery, AnalysisResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAnalysisResponseByIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetAnalysisResponseByIdHandler(IUnitOfWork unitOfWork, ILogger<GetAnalysisResponseByIdHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<AnalysisResponseDto?> Handle(GetAnalysisResponseByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = await _unitOfWork.Repository
                .AnalysisResponses
                .SingleAsNoTrackingAsync(x => x.Id == request.Id,
                include: x => x.Include(y => y.GeneralText).Include(y => y.Invoice).ThenInclude(x => x.Items));

            return _mapper.Map<AnalysisResponseDto>(query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return null;
    }
}