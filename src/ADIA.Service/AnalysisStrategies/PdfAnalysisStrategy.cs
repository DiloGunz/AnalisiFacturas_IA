using ADIA.AzureIa.Proxy.Responses;
using ADIA.AzureIa.Proxy.Services;
using ADIA.Model.DataTransfer.Commands.AnalysisCommands;
using ADIA.Model.DataTransfer.IaResponses;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Responses;
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
        var azureResult = await AnalyzePdfWithAzureAI(command);
        if (!azureResult.Success)
        {
            return CreateFailureResult(azureResult);
        }

        var openIAResult = await AnalyzeTextWithOpenAI(azureResult.Result);
        if (!openIAResult.Success)
        {
            return CreateFailureResult(openIAResult);
        }

        return DeserializeAndPrepareResult(openIAResult);
    }

    private async Task<AnalysisAzureIAResponse> AnalyzePdfWithAzureAI(CreateAnalysisCommand command)
    {
        var azureIARequest = new AnalysisAzureIARequest { File = command.File };
        return await _analysisPdfAzureAIService.ProcessPdfAsync(azureIARequest);
    }

    private async Task<AnalysisOpenIAResponse> AnalyzeTextWithOpenAI(string extractedText)
    {
        var openIARequest = new AnalysisOpenIARequest
        {
            PromptSystem = PromtsIA.PromtSystem,
            PromptUser = string.Join("\n", PromtsIA.PromptPdf, extractedText)
        };
        return await _analysisTextOpenAI.ProcessAsync(openIARequest);
    }

    private AnalysisIAResultDto CreateFailureResult(AnalysisOpenIAResponse response)
    {
        return new AnalysisIAResultDto
        {
            Success = false,
            DocumentType = EntityEnums.DocumentType.Undefined,
            Start = response.Start,
            End = response.End,
            Message = response.Result
        };
    }

    private AnalysisIAResultDto CreateFailureResult(AnalysisAzureIAResponse response)
    {
        return new AnalysisIAResultDto
        {
            Success = false,
            DocumentType = EntityEnums.DocumentType.Undefined,
            Start = response.Start,
            End = response.End,
            Message = response.Result
        };
    }

    private AnalysisIAResultDto DeserializeAndPrepareResult(AnalysisOpenIAResponse response)
    {
        var result = JsonSerializer.Deserialize<AnalysisIAResultDto>(response.Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (result == null)
        {
            throw new Exception("Error al leer los datos devueltos por IA");
        }

        result.Success = true;
        result.Start = response.Start;
        result.End = response.End;
        MapDataToDto(result);

        return result;
    }

    private void MapDataToDto(AnalysisIAResultDto result)
    {
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
    }
}