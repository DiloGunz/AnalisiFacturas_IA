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

public class ImageAnalysisStrategy : IAnalysisStrategy
{
    private readonly IAnalysisImageOpenAIService _analysisImageOpenAIService;

    public ImageAnalysisStrategy(IAnalysisImageOpenAIService analysisImageOpenAIService)
    {
        _analysisImageOpenAIService = analysisImageOpenAIService ?? throw new ArgumentNullException(nameof(analysisImageOpenAIService));
    }

    public async Task<AnalysisIAResultDto> AnalyzeAsync(CreateAnalysisCommand command)
    {
        var openIARequest = MapCommandToRequest(command);
        var resultOpenIA = await _analysisImageOpenAIService.ProcessAsync(openIARequest);

        if (!resultOpenIA.Success)
        {
            return CreateFailureResult(resultOpenIA);
        }

        return await ParseResult(resultOpenIA);
    }

    private AnalysisOpenIARequest MapCommandToRequest(CreateAnalysisCommand command)
    {
        return new AnalysisOpenIARequest
        {
            File = command.File,
            PromptSystem = PromtsIA.PromptSystem,  // Fixed a potential typo in property name
            PromptUser = PromtsIA.PromptImagen
        };
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

    private async Task<AnalysisIAResultDto> ParseResult(AnalysisOpenIAResponse response)
    {
        var result = JsonSerializer.Deserialize<AnalysisIAResultDto>(response.Result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
        {
            throw new Exception("Error al leer los datos devueltos por IA");
        }

        result.Success = true;
        result.Start = response.Start;
        result.End = response.End;
        result.Data = MapResultData(result);

        return result;
    }

    private object MapResultData(AnalysisIAResultDto result)
    {
        switch (result.DocumentType)
        {
            case EntityEnums.DocumentType.Undefined:
                return result.Data.MapTo<UndefinedIADto>();
            case EntityEnums.DocumentType.Invoice:
                return result.Data.MapTo<InvoiceIADto>();
            case EntityEnums.DocumentType.GeneralText:
                return result.Data.MapTo<GeneralTextIADto>();
            default:
                throw new Exception("Tipo de documento no soportado en la respuesta devuelta por IA");
        }
    }
}