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

/// <summary>
/// Implementa la estrategia de análisis de imágenes, utilizando servicios de análisis de OpenAI para procesar imágenes.
/// </summary>
public class ImageAnalysisStrategy : IAnalysisStrategy
{
    private readonly IAnalysisImageOpenAIService _analysisImageOpenAIService;

    /// <summary>
    /// Inicializa una nueva instancia de la clase ImageAnalysisStrategy con el servicio necesario para el análisis de imágenes.
    /// </summary>
    /// <param name="analysisImageOpenAIService">Servicio para el análisis de imágenes usando OpenAI.</param>
    public ImageAnalysisStrategy(IAnalysisImageOpenAIService analysisImageOpenAIService)
    {
        _analysisImageOpenAIService = analysisImageOpenAIService ?? throw new ArgumentNullException(nameof(analysisImageOpenAIService));
    }

    /// <summary>
    /// Analiza de manera asíncrona una imagen utilizando OpenAI, procesando la imagen y analizando el contenido visual.
    /// </summary>
    /// <param name="command">Comando que contiene la imagen a analizar.</param>
    /// <returns>El resultado del análisis como un DTO que incluye si fue exitoso y detalles del análisis realizado.</returns>
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

    /// <summary>
    /// Mapea el comando recibido a una solicitud de análisis de imagen adecuada para el servicio de OpenAI.
    /// </summary>
    /// <param name="command">Comando con la información necesaria para formar la solicitud.</param>
    /// <returns>Una solicitud de análisis de imagen formateada para el servicio OpenAI.</returns>
    private AnalysisOpenIARequest MapCommandToRequest(CreateAnalysisCommand command)
    {
        return new AnalysisOpenIARequest
        {
            File = command.File,
            PromptSystem = PromtsIA.PromtSystem, 
            PromptUser = PromtsIA.PromptImagen
        };
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
    /// Analiza y prepara el resultado del análisis deserializando la respuesta y mapeando los datos obtenidos.
    /// </summary>
    /// <param name="response">La respuesta del análisis de OpenAI.</param>
    /// <returns>El DTO de resultado de análisis con todos los detalles relevantes y ajustados.</returns>
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

    /// <summary>
    /// Mapea los datos del resultado del análisis a estructuras de datos específicas basadas en el tipo de documento identificado.
    /// </summary>
    /// <param name="result">El resultado del análisis que se está mapeando.</param>
    /// <returns>El dato mapeado específico del tipo de documento.</returns>
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