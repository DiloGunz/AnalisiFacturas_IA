using ADIA.Model.DataTransfer.Commands.AnalysisCommands;
using ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;
using ADIA.Model.DataTransfer.IaResponses;
using ADIA.Model.Domain.Entities;
using ADIA.Service.AnalysisStrategies.Interfaces;
using ADIA.Service.Validations.Extensions.Extensions;
using ADIA.Shared.Enums;
using ADIA.Shared.Extensions;
using ADIA.Shared.Response;
using ADIA.Uow.Interfaces;
using AutoMapper;
using Azure.Core;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ADIA.Service.EventHandlers.AnalysisEventHandlers;

/// <summary>
/// Maneja la creación y análisis de documentos procesando un comando CreateAnalysisCommand dado.
/// El manejador utiliza varios servicios como una unidad de trabajo, registro de eventos, validador, mapeador y un resolvedor de estrategias de análisis.
/// </summary>
public class CreateAnalysisHandler : IRequestHandler<CreateAnalysisCommand, AppResponse<AnalysisResponseDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CreateAnalysisHandler> _logger;
    private readonly IValidator<CreateAnalysisCommand> _validator;
    private readonly IAnalysisStrategyResolver _analysisStrategyResolver;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase CreateAnalysisHandler.
    /// </summary>
    /// <param name="uow">Unidad de trabajo para transacciones de base de datos.</param>
    /// <param name="logger">Registrador para registrar errores e información.</param>
    /// <param name="validator">Validador para validar los comandos entrantes.</param>
    /// <param name="mapper">Mapeador para convertir entre entidades y DTOs.</param>
    /// <param name="analysisStrategyResolver">Resolvedor para determinar la estrategia de análisis adecuada basada en el tipo de archivo.</param>
    public CreateAnalysisHandler(
        IUnitOfWork uow,
        ILogger<CreateAnalysisHandler> logger,
        IValidator<CreateAnalysisCommand> validator,
        IMapper mapper,
        IAnalysisStrategyResolver analysisStrategyResolver)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _analysisStrategyResolver = analysisStrategyResolver ?? throw new ArgumentNullException(nameof(analysisStrategyResolver));
    }

    /// <summary>
    /// Maneja el procesamiento de un CreateAnalysisCommand de manera asíncrona.
    /// </summary>
    /// <param name="request">El comando a procesar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona y devuelve una respuesta que encapsula el resultado.</returns>
    public async Task<AppResponse<AnalysisResponseDto>> Handle(CreateAnalysisCommand request, CancellationToken cancellationToken)
    {
        var response = AppResponse<AnalysisResponseDto>.CreateDefault();
        try
        {
            ValidateRequest(request, cancellationToken, response);
            var analysis = await CreateEntityAsync(request);
            var analysisResult = await AnalyzeDocument(analysis, request);
            return await ProcessAnalysisResponse(analysisResult, analysis.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing analysis command.");
            return response.Failure(ex.Message);
        }
    }

    /// <summary>
    /// Valida la solicitud de creación de análisis asegurándose de que cumpla con los criterios especificados en el validador. Si la validación falla, se lanza una excepción con los detalles del error.
    /// </summary>
    /// <param name="request">Comando de creación de análisis que se valida.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <param name="response">Respuesta de la aplicación que se actualizará en caso de error.</param>
    private async void ValidateRequest(CreateAnalysisCommand request, CancellationToken cancellationToken, AppResponse<AnalysisResponseDto> response)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.GetMessageErrors().FirstOrDefault());
        }
    }

    /// <summary>
    /// Crea una entidad de análisis en la base de datos a partir del comando de solicitud, configurando detalles como la fecha de análisis y la información del archivo.
    /// </summary>
    /// <param name="request">Comando de solicitud que contiene los datos del archivo a analizar.</param>
    /// <returns>La entidad de análisis creada.</returns>
    private async Task<Analysis> CreateEntityAsync(CreateAnalysisCommand request)
    {
        var entity = new Analysis()
        {
            AnalysisDate = DateTime.Now,
            FileBase64 = ConvertToBase64(request.File),
            FileExtension = request.FileExtension.ToUpper().Trim(),
            FileName = request.FileName.ToUpper().Trim(),
            FileType = request.FileExtension.GetFileType(),
        };
        await _uow.Repository.Analysis.AddAsync(entity);
        await _uow.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Convierte los datos de un archivo a su representación en formato BASE64.
    /// </summary>
    /// <param name="file">Datos binarios del archivo a convertir.</param>
    /// <returns>Una cadena que representa el archivo en formato BASE64.</returns>
    private string ConvertToBase64(byte[] file)
    {
        var fileBase64 = Convert.ToBase64String(file);
        if (string.IsNullOrWhiteSpace(fileBase64))
        {
            throw new Exception("El archivo no se pudo convertir a BASE64.");
        }
        return fileBase64.Trim();
    }

    /// <summary>
    /// Realiza el análisis del documento utilizando la estrategia de análisis adecuada resuelta para el tipo de archivo proporcionado.
    /// </summary>
    /// <param name="analysis">Entidad de análisis que contiene los detalles del archivo.</param>
    /// <param name="request">Comando de solicitud que contiene los datos y configuración del análisis.</param>
    /// <returns>El resultado del análisis del documento.</returns>
    private async Task<AnalysisResponse> AnalyzeDocument(Analysis analysis, CreateAnalysisCommand request)
    {
        var strategy = _analysisStrategyResolver.Resolve(analysis.FileType);
        var analysisResult = await strategy.AnalyzeAsync(request);
        return await CreateAnalysisResponseAsync(analysis.Id, analysisResult);
    }

    /// <summary>
    /// Procesa la respuesta de análisis y mapea a DTO si el análisis fue exitoso. Si el análisis indica un fallo, se lanza una excepción.
    /// </summary>
    /// <param name="analysisResponse">Resultado del análisis del documento.</param>
    /// <param name="analysisId">Identificador de la entidad de análisis asociada.</param>
    /// <returns>Una respuesta de aplicación encapsulando el DTO de la respuesta de análisis.</returns>
    private async Task<AppResponse<AnalysisResponseDto>> ProcessAnalysisResponse(AnalysisResponse analysisResponse, long analysisId)
    {
        if (!analysisResponse.IsSuccess)
        {
            throw new InvalidOperationException("Analysis response indicates failure: " + analysisResponse.Message);
        }
        var analysisResponseDto = _mapper.Map<AnalysisResponseDto>(analysisResponse);
        return new AppResponse<AnalysisResponseDto>().Success(analysisResponseDto);
    }

    /// <summary>
    /// Crea la respuesta de análisis en la base de datos a partir de los resultados obtenidos y asocia con el ID de análisis correspondiente.
    /// </summary>
    /// <param name="analysisId">Identificador del análisis para el cual se crea la respuesta.</param>
    /// <param name="resultDto">DTO de los resultados del análisis para mapear a la entidad de respuesta.</param>
    /// <returns>La entidad de respuesta de análisis creada.</returns>
    private async Task<AnalysisResponse> CreateAnalysisResponseAsync(long analysisId, AnalysisIAResultDto resultDto)
    {
        var analysisResponse = new AnalysisResponse()
        {
            IdAnalysis = analysisId,
            IsSuccess = resultDto.Success,
            DocumentType = resultDto.DocumentType,
            Ia = EntityEnums.Ia.OpenIa,
            StartAnalysis = resultDto.Start,
            EndAnalysis = resultDto.End,
            ResponseTime = (decimal)(resultDto.End - resultDto.Start).TotalMilliseconds,
            Message = resultDto.Message.ToUpperTrim(),
        };
        analysisResponse.IdAnalysis = analysisId;
        await _uow.Repository.AnalysisResponses.AddAsync(analysisResponse);
        await _uow.SaveChangesAsync();
        return analysisResponse;
    }
}