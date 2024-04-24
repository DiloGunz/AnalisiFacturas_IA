using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Model.DataTransfer.Queries;
using ADIA.Model.Domain.Entities;
using ADIA.Uow.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ADIA.Service.QueryHandlers.AnalysisQueryHandlers;

/// <summary>
/// Maneja la consulta para obtener una lista de análisis basada en criterios específicos, utilizando una unidad de trabajo para las operaciones de base de datos y un mapeador para convertir entidades a DTOs.
/// </summary>
public class GetAnalysisListHandler : IRequestHandler<GetAnalysisListQuery, IEnumerable<AnalysisListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAnalysisListHandler> _logger;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase GetAnalysisListHandler.
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para acceder a repositorios y realizar operaciones de base de datos.</param>
    /// <param name="logger">Registrador para capturar y registrar errores durante la ejecución.</param>
    /// <param name="mapper">Mapeador para convertir de entidades de análisis a DTOs de lista de análisis.</param>
    public GetAnalysisListHandler(IUnitOfWork unitOfWork, ILogger<GetAnalysisListHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Maneja la solicitud de consulta para obtener una lista de análisis, filtrando y mapeando los resultados a DTOs.
    /// </summary>
    /// <param name="request">La consulta que contiene los criterios de búsqueda y filtro para los análisis.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve una lista enumerable de DTOs de análisis.</returns>
    public async Task<IEnumerable<AnalysisListDto>> Handle(GetAnalysisListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Analysis> analysisList = await FetchAnalysisListAsync(request, cancellationToken);
        return _mapper.Map<IEnumerable<AnalysisListDto>>(analysisList);
    }

    /// <summary>
    /// Recupera una lista de análisis de la base de datos según los filtros especificados en la consulta, aplicando criterios de búsqueda y rangos de fechas.
    /// </summary>
    /// <param name="request">La consulta que especifica los criterios de filtro para la búsqueda de análisis.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve una lista enumerable de entidades de análisis.</returns>
    private async Task<IEnumerable<Analysis>> FetchAnalysisListAsync(GetAnalysisListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _unitOfWork.Repository.Analysis.GetAllAsNoTrackingAsync(
                predicate: x => x.AnalysisDate >= request.DtFrom && x.AnalysisDate <= request.DtTo && x.FileName.Contains(request.SearchString),
                include: x => x.Include(y => y.AnalysisResponse),
                orderBy: x => x.OrderByDescending(y => y.AnalysisDate));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch analysis list.");
            throw; 
        }
    }
}