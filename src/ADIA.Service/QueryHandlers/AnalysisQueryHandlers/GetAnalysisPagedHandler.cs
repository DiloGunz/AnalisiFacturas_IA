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

/// <summary>
/// Maneja la solicitud para obtener una lista paginada de análisis basada en criterios específicos, utilizando una unidad de trabajo para las operaciones de base de datos y un mapeador para convertir entidades a DTOs.
/// </summary>
public class GetAnalysisPagedHandler : IRequestHandler<GetAnalysisPagedQuery, DataCollection<AnalysisListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAnalysisPagedHandler> _logger;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase GetAnalysisPagedHandler.
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para acceder a repositorios y realizar operaciones de base de datos.</param>
    /// <param name="logger">Registrador para capturar y registrar errores durante la ejecución.</param>
    /// <param name="mapper">Mapeador para convertir de entidades de análisis a DTOs de lista de análisis paginada.</param>
    public GetAnalysisPagedHandler(IUnitOfWork unitOfWork, ILogger<GetAnalysisPagedHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Maneja la solicitud de consulta para obtener una lista paginada de análisis, filtrando y mapeando los resultados a DTOs.
    /// </summary>
    /// <param name="request">La consulta que contiene los criterios de búsqueda y filtro para los análisis.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve una colección de datos de DTOs de análisis.</returns>
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
            throw; 
        }
    }

    /// <summary>
    /// Recupera una colección de datos paginada de análisis de la base de datos según los filtros especificados en la consulta.
    /// </summary>
    /// <param name="request">La consulta que especifica los criterios de filtro y paginación para la búsqueda de análisis.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve una colección de datos paginada de entidades de análisis.</returns>
    private async Task<DataCollection<Analysis>> FetchPagedAnalysisAsync(GetAnalysisPagedQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository.Analysis.GetPagedAsync(
            request.Page,
            request.PageSize,
            x => x.OrderByDescending(y => y.AnalysisDate),
            predicate: x => x.AnalysisDate >= request.DtFrom && x.AnalysisDate <= request.DtTo.GetEndDay() && x.FileName.Contains(request.SearchString),
            include: x => x.Include(y => y.AnalysisResponse));
    }

    /// <summary>
    /// Mapea el resultado de la consulta paginada a DTOs de lista de análisis.
    /// </summary>
    /// <param name="queryResult">El resultado de la consulta paginada obtenido de la base de datos.</param>
    /// <returns>Una colección de datos de DTOs de lista de análisis que representa el resultado paginado.</returns>
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