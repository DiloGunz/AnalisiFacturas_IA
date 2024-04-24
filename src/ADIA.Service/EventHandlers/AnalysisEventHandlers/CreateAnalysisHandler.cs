using ADIA.Model.DataTransfer.Commands.AnalysisCommands;
using ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;
using ADIA.Model.DataTransfer.IaResponses;
using ADIA.Model.Domain.Entities;
using ADIA.Service.AnalysisStrategies.Interfaces;
using ADIA.Service.Validations.Extensions.Extensions;
using ADIA.Shared.Extensions;
using ADIA.Shared.Response;
using ADIA.Uow.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ADIA.Service.EventHandlers.AnalysisEventHandlers;

public class CreateAnalysisHandler : IRequestHandler<CreateAnalysisCommand, AppResponse<AnalysisResponseDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CreateAnalysisHandler> _logger;
    private readonly IValidator<CreateAnalysisCommand> _validator;
    private readonly IAnalysisStrategyResolver _analysisStrategyResolver;
    private readonly IMapper _mapper;

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

    private async void ValidateRequest(CreateAnalysisCommand request, CancellationToken cancellationToken, AppResponse<AnalysisResponseDto> response)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.GetMessageErrors().FirstOrDefault());
        }
    }

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

    private string ConvertToBase64(byte[] file)
    {
        var fileBase64 = Convert.ToBase64String(file);
        if (string.IsNullOrWhiteSpace(fileBase64))
        {
            throw new Exception("El archivo no se pudo convertir a BASE64.");
        }
        return fileBase64.Trim();
    }

    private async Task<AnalysisResponse> AnalyzeDocument(Analysis analysis, CreateAnalysisCommand request)
    {
        var strategy = _analysisStrategyResolver.Resolve(analysis.FileType);
        var analysisResult = await strategy.AnalyzeAsync(request);
        return await CreateAnalysisResponseAsync(analysis.Id, analysisResult);
    }

    private Task<AppResponse<AnalysisResponseDto>> ProcessAnalysisResponse(AnalysisResponse analysisResponse, long analysisId)
    {
        if (!analysisResponse.IsSuccess)
        {
            throw new InvalidOperationException("Analysis response indicates failure: " + analysisResponse.Message);
        }
        var analysisResponseDto = _mapper.Map<AnalysisResponseDto>(analysisResponse);
        return Task.FromResult(new AppResponse<AnalysisResponseDto>().Success(analysisResponseDto));
    }

    private async Task<AnalysisResponse> CreateAnalysisResponseAsync(long analysisId, AnalysisIAResultDto resultDto)
    {
        var analysisResponse = _mapper.Map<AnalysisResponse>(resultDto);
        analysisResponse.IdAnalysis = analysisId;
        await _uow.Repository.AnalysisResponses.AddAsync(analysisResponse);
        await _uow.SaveChangesAsync();
        return analysisResponse;
    }
}