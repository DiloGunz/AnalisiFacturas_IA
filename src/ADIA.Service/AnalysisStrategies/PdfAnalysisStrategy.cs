using ADIA.AzureIa.Proxy.Responses;
using ADIA.AzureIa.Proxy.Services;
using ADIA.Model.DataTransfer.Commands.AnalysisCommands;
using ADIA.Model.DataTransfer.IaResponses;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Services;
using ADIA.Service.AnalysisStrategies.Interfaces;
using ADIA.Service.IaServices.Promts;
using ADIA.Shared.Enums;
using ADIA.Shared.Mapping;
using System.Text.Json;

namespace ADIA.Service.AnalysisStrategies;

public class PdfAnalysisStrategy : IAnalysisStrategy
{
    private readonly IAnalysisPdfAzureAIService _analysisPdfAzureAIService;
    private readonly IAnalysisTextOpenAI _analysisTextOpenAI;

    public PdfAnalysisStrategy(IAnalysisPdfAzureAIService analysisPdfAzureAIService, IAnalysisTextOpenAI analysisTextOpenAI)
    {
        _analysisPdfAzureAIService = analysisPdfAzureAIService ?? throw new ArgumentNullException(nameof(analysisPdfAzureAIService));
        _analysisTextOpenAI = analysisTextOpenAI ?? throw new ArgumentNullException(nameof(analysisTextOpenAI));
    }

    public async Task<AnalysisIAResultDto> AnalyzeAsync(CreateAnalysisCommand command)
    {
        var azureIARequest = new AnalysisAzureIARequest()
        {
            File = command.File
        };

        var resultAzureIa = await _analysisPdfAzureAIService.ProcessPdfAsync(azureIARequest);

        if (!resultAzureIa.Success)
        {
            return new AnalysisIAResultDto()
            {
                Success = false,
                DocumentType = EntityEnums.DocumentType.Undefined,
                Start = resultAzureIa.Start,
                End = resultAzureIa.End,
                Message = resultAzureIa.Result
            };
        }

        var openIARequest = new AnalysisOpenIARequest()
        {
            PromptSystem = PromtsIA.PromtSystem,
            PromptUser = string.Join("\n", PromtsIA.PromptPdf, resultAzureIa.Result)
        };

        var resultOpenIA = await _analysisTextOpenAI.ProcessAsync(openIARequest);

        if (!resultOpenIA.Success)
        {
            return new AnalysisIAResultDto()
            {
                Success = false,
                DocumentType = EntityEnums.DocumentType.Undefined,
                Start = resultOpenIA.Start,
                End = resultOpenIA.End,
                Message = resultOpenIA.Result
            };
        }

        var result = JsonSerializer.Deserialize<AnalysisIAResultDto>(resultOpenIA.Result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });

        if (result == null)
        {
            throw new Exception("Error al leer los datos devueltos por IA");
        }

        result.Success = true;
        result.Start = resultOpenIA.Start;
        result.End = resultOpenIA.End;

        switch (result.DocumentType)
        {
            case EntityEnums.DocumentType.Undefined:
                result.Data = result.Data.MapTo<UndefinedIADto>();
                break;
            case EntityEnums.DocumentType.Invoice:
                result.Data = result.Data.MapTo<InvoiceIADto>();
                break;
            case EntityEnums.DocumentType.GeneralText:
                result.Data = result.Data.MapTo<GeneralTextIADto>();
                break;
            default:
                throw new Exception("Error en el tipo de archivo en la respuesta devuelta por IA.");
        }

        return result;
    }
}