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

/// <summary>
/// Implementa la estrategia de análisis de documentos PDF, utilizando servicios de análisis de texto de Azure AI y OpenAI.
/// </summary>
public class PdfAnalysisStrategy : IAnalysisStrategy
{
    private readonly IAnalysisPdfAzureAIService _analysisPdfAzureAIService;
    private readonly IAnalysisTextOpenAI _analysisTextOpenAI;

    /// <summary>
    /// Inicializa una nueva instancia de la clase PdfAnalysisStrategy con los servicios necesarios para el análisis.
    /// </summary>
    /// <param name="analysisPdfAzureAIService">Servicio para el análisis de documentos PDF usando Azure AI.</param>
    /// <param name="analysisTextOpenAI">Servicio para el análisis de texto usando OpenAI.</param>
    public PdfAnalysisStrategy(IAnalysisPdfAzureAIService analysisPdfAzureAIService, IAnalysisTextOpenAI analysisTextOpenAI)
    {
        _analysisPdfAzureAIService = analysisPdfAzureAIService ?? throw new ArgumentNullException(nameof(analysisPdfAzureAIService));
        _analysisTextOpenAI = analysisTextOpenAI ?? throw new ArgumentNullException(nameof(analysisTextOpenAI));
    }

    /// <summary>
    /// Analiza de manera asíncrona un documento PDF utilizando primero Azure AI para extraer texto y luego OpenAI para analizar el texto extraído.
    /// </summary>
    /// <param name="command">Comando que contiene el archivo PDF a analizar.</param>
    /// <returns>El resultado del análisis como un DTO que incluye si fue exitoso y detalles del documento analizado.</returns>
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

    /// <summary>
    /// Realiza el análisis del documento PDF utilizando el servicio Azure AI, enviando el archivo y esperando el resultado del análisis.
    /// </summary>
    /// <param name="command">Comando que contiene el archivo PDF y la información necesaria para procesarlo.</param>
    /// <returns>Una tarea que retorna la respuesta del análisis realizado por Azure AI.</returns>
    private async Task<AnalysisAzureIAResponse> AnalyzePdfWithAzureAI(CreateAnalysisCommand command)
    {
        var azureIARequest = new AnalysisAzureIARequest { File = command.File };
        return await _analysisPdfAzureAIService.ProcessPdfAsync(azureIARequest);
    }

    /// <summary>
    /// Realiza un análisis adicional del texto extraído usando el servicio OpenAI, enviando el texto extraído para su evaluación.
    /// </summary>
    /// <param name="extractedText">El texto extraído del documento PDF por Azure AI.</param>
    /// <returns>Una tarea que retorna la respuesta del análisis realizado por OpenAI.</returns>
    private async Task<AnalysisOpenIAResponse> AnalyzeTextWithOpenAI(string extractedText)
    {
        var openIARequest = new AnalysisOpenIARequest
        {
            PromptSystem = PromtsIA.PromtSystem,
            PromptUser = string.Join("\n", PromtsIA.PromptPdf, extractedText)
        };
        return await _analysisTextOpenAI.ProcessAsync(openIARequest);
    }

    /// <summary>
    /// Crea un resultado de análisis fallido a partir de la respuesta de OpenAI, capturando el mensaje de error y otros detalles relevantes.
    /// </summary>
    /// <param name="response">La respuesta del análisis de OpenAI que indica un fracaso.</param>
    /// <returns>Un DTO que representa un resultado fallido del análisis.</returns>
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

    /// <summary>
    /// Crea un resultado de análisis fallido a partir de la respuesta de Azure AI, capturando el mensaje de error y otros detalles relevantes.
    /// </summary>
    /// <param name="response">La respuesta del análisis de Azure AI que indica un fracaso.</param>
    /// <returns>Un DTO que representa un resultado fallido del análisis.</returns>
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

    /// <summary>
    /// Deserializa y prepara el resultado final del análisis, asegurando que todos los campos necesarios estén correctamente establecidos.
    /// </summary>
    /// <param name="response">La respuesta del análisis de OpenAI que necesita ser deserializada y mapeada a un DTO.</param>
    /// <returns>Un DTO que representa el resultado exitoso del análisis.</returns>
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

    /// <summary>
    /// Mapea los datos del resultado deserializado a un DTO específico, dependiendo del tipo de documento detectado.
    /// </summary>
    /// <param name="result">El resultado del análisis que se está mapeando.</param>
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