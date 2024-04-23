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
            var validator = await _validator.ValidateAsync(request, cancellationToken);

            if (!validator.IsValid) return response.Validation(validator.GetMessageErrors());

            var analysis = await CreateEntityAsync(request);

            var strategy = _analysisStrategyResolver.Resolve(analysis.FileType);
            var analysisResult = await strategy.AnalyzeAsync(request);

            var analysisResponse = await CreateAnalysisResponseAsync(analysis.Id, analysisResult);

            if (!analysisResponse.IsSuccess)
            {
                return response.Failure(analysisResponse.Message);
            }
            return response.Success(_mapper.Map<AnalysisResponseDto>(analysisResponse));
        }

        catch (Exception ex)
        {
            response.Failure(ex.Message);
            _logger.LogError(ex, ex.Message);
        }
        return response;
    }


    private async Task<Analysis> CreateEntityAsync(CreateAnalysisCommand request)
    {
        var fileBase64 = Convert.ToBase64String(request.File);

        if (string.IsNullOrWhiteSpace(fileBase64))
        {
            throw new Exception("El archivo no se pudo convertir a BASE64");
        }

        var entity = new Analysis()
        {
            AnalysisDate = DateTime.Now,
            FileBase64 = fileBase64.Trim(),
            FileExtension = request.FileExtension.ToUpperTrim(),
            FileName = request.FileName.ToUpperTrim(),
            FileType = request.FileExtension.GetFileType(),
        };

        await _uow.Repository.Analysis.AddAsync(entity);

        await _uow.SaveChangesAsync();

        return entity;
    }

    private async Task<AnalysisResponse> CreateAnalysisResponseAsync(long idAnalysis, AnalysisIAResultDto request)
    {
        var entity = new AnalysisResponse()
        {
            IdAnalysis = idAnalysis,
            IsSuccess = request.Success,
            DocumentType = request.DocumentType,
            Ia = EntityEnums.Ia.OpenIa,
            StartAnalysis = request.Start,
            EndAnalysis = request.End,
            ResponseTime = (decimal)(request.End - request.Start).TotalMilliseconds,
            Message = request.Message.ToUpperTrim(),
        };

        await _uow.Repository.AnalysisResponses.AddAsync(entity);

        if (request.DocumentType == EntityEnums.DocumentType.Invoice)
        {
            entity.Invoice = GetInvoice(request.Data);
        }
        else if (request.DocumentType == EntityEnums.DocumentType.GeneralText)
        {
            entity.GeneralText = GetGeneralText(request.Data);
        }
        else if (request.DocumentType == EntityEnums.DocumentType.Undefined)
        {
            entity.IsSuccess = false;
        }

        await _uow.SaveChangesAsync();

        return entity;
    }

    private Invoice GetInvoice(object request)
    {
        var invoiceDto = request as InvoiceIADto;

        if (invoiceDto is null)
        {
            throw new Exception("Error al crear la facatura en BD.");
        }

        var invoice = new Invoice()
        {
            Currency = invoiceDto.Currency.ToUpperTrim(),
            CustomerAddress = invoiceDto.CustomerAddress.ToUpperTrim(),
            CustomerName = invoiceDto.CustomerName.ToUpperTrim(),
            InvoiceDate = invoiceDto.InvoiceDateTime.ToUpperTrim(),
            InvoiceNumber = invoiceDto.InvoiceNumber.ToUpperTrim(),
            SupplierAddress = invoiceDto.SupplierAddress.ToUpperTrim(),
            SupplierName = invoiceDto.SupplierName.ToUpperTrim(),
            TotalAmmount = invoiceDto.TotalAmmount,
            Items = new List<ItemInvoice>()
        };

        foreach (var item in invoiceDto.Items)
        {
            var itemInvoice = new ItemInvoice()
            {
                Description = item.Description.ToUpperTrim(),
                Quantity = item.Quantity,
                TotalAmount = item.TotalAmount,
                UnitPrice = item.UnitPrice,
            };

            invoice.Items.Add(itemInvoice);
        }

        return invoice;
    }

    private GeneralText GetGeneralText(object request)
    {
        var generalTextDto = request as GeneralTextIADto;

        if (generalTextDto == null)
        {
            throw new Exception("Error al crear el texto general en BD.");
        }

        return new GeneralText()
        {
            Description = generalTextDto.Description.ToUpperTrim(),
            Sentiment = generalTextDto.Sentiment.ToUpperTrim(),
            Summary = generalTextDto.Summary.ToUpperTrim(),
        };
    }
}