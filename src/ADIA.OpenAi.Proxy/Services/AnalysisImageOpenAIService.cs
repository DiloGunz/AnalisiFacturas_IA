using ADIA.OpenAi.Proxy.Config.Models;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Utils;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Models;
using static OpenAI_API.Chat.ChatMessage;

namespace ADIA.OpenAi.Proxy.Services;

/// <summary>
/// Servicio para procesar análisis de imágenes usando OpenAI. Gestiona la configuración, validación de solicitudes y manejo de excepciones.
/// </summary>
public class AnalysisImageOpenAIService : IAnalysisImageOpenAIService
{
    private readonly OpenIaConfig _openIaConfig;
    private readonly ILogger<AnalysisImageOpenAIService> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de análisis de imágenes.
    /// </summary>
    /// <param name="openIaConfig">Configuración para la API de OpenAI.</param>
    /// <param name="logger">Logger para registrar eventos y errores.</param>
    public AnalysisImageOpenAIService(OpenIaConfig openIaConfig, ILogger<AnalysisImageOpenAIService> logger)
    {
        _openIaConfig = openIaConfig ?? throw new ArgumentNullException(nameof(openIaConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Procesa una solicitud de análisis de imágenes de manera asíncrona.
    /// </summary>
    /// <param name="request">La solicitud de análisis de imagen.</param>
    /// <returns>Una tarea que retorna un objeto AnalysisOpenIAResponse que contiene los resultados del análisis.</returns>

    public async Task<AnalysisOpenIAResponse> ProcessAsync(AnalysisOpenIARequest request)
    {
        var analysisResponse = new AnalysisOpenIAResponse() { Start = DateTime.Now };

        try
        {
            ValidateRequest(request);
            var api = GetOpenAIApi();
            var response = await PerformImageAnalysis(api, request);
            PopulateAnalysisResponse(response, analysisResponse);
        }
        catch (Exception ex)
        {
            HandleException(ex, analysisResponse);
        }

        analysisResponse.End = DateTime.Now;
        return analysisResponse;
    }

    /// <summary>
    /// Obtiene la instancia de la API de OpenAI configurada con la clave API.
    /// </summary>
    /// <returns>Instancia de OpenAIAPI configurada.</returns>
    private OpenAIAPI GetOpenAIApi()
    {
        return new OpenAIAPI(_openIaConfig.ApiKey);
    }

    /// <summary>
    /// Valida la solicitud de análisis para asegurar que contiene una imagen válida.
    /// </summary>
    /// <param name="request">La solicitud de análisis que se está validando.</param>
    private void ValidateRequest(AnalysisOpenIARequest request)
    {
        if (request.File == null || request.File.Length == 0)
            throw new ArgumentException("Se ingresó una imagen inválida");
    }

    /// <summary>
    /// Realiza el análisis de la imagen usando la API de OpenAI y retorna la respuesta.
    /// </summary>
    /// <param name="api">La API de OpenAI utilizada para el análisis.</param>
    /// <param name="request">La solicitud que contiene la imagen y los mensajes de prompt.</param>
    /// <returns>Una tarea que retorna la respuesta del análisis como cadena.</returns>
    private async Task<string> PerformImageAnalysis(OpenAIAPI api, AnalysisOpenIARequest request)
    {
        var chat = api.Chat.CreateConversation();
        chat.Model = Model.GPT4_Vision;
        chat.AppendSystemMessage(request.PromptSystem);
        chat.AppendUserInput(request.PromptUser, ImageInput.FromImageBytes(request.File));
        var response = await chat.GetResponseFromChatbotAsync();
        if (string.IsNullOrWhiteSpace(response))
            throw new InvalidOperationException("La respuesta de OPENIA fue nula o vacía");
        return response;
    }

    /// <summary>
    /// Puebla la respuesta de análisis basándose en la respuesta obtenida de la API de OpenAI.
    /// </summary>
    /// <param name="response">La respuesta obtenida del análisis de imagen.</param>
    /// <param name="analysisResponse">La respuesta de análisis que se está poblado.</param>
    private void PopulateAnalysisResponse(string response, AnalysisOpenIAResponse analysisResponse)
    {
        ValidateOpenIaResponse.Process(response);
        analysisResponse.Success = true;
        analysisResponse.Result = FixResponse.Process(response);
    }

    /// <summary>
    /// Maneja las excepciones capturadas durante el procesamiento, registrando el error y actualizando la respuesta de análisis.
    /// </summary>
    /// <param name="ex">La excepción capturada durante el procesamiento.</param>
    /// <param name="analysisResponse">La respuesta de análisis que se actualizará con los detalles del fallo.</param>
    private void HandleException(Exception ex, AnalysisOpenIAResponse analysisResponse)
    {
        var message = ValidateOpenIaResponse.GetMessageOfBadRequestResponse(ex.Message);
        _logger.LogError(ex, ex.Message);
        analysisResponse.Success = false;
        analysisResponse.Result = message;
    }
}