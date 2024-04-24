using ADIA.OpenAi.Proxy.Config.Models;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Utils;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace ADIA.OpenAi.Proxy.Services;

/// <summary>
/// Servicio para procesar análisis de texto usando OpenAI. Gestiona la configuración, validación de solicitudes y manejo de excepciones.
/// </summary>
public class AnalysisTextOpenAI : IAnalysisTextOpenAI
{
    private readonly OpenIaConfig _openIaConfig;
    private readonly ILogger<AnalysisTextOpenAI> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de análisis de texto.
    /// </summary>
    /// <param name="openIaConfig">Configuración para la API de OpenAI.</param>
    /// <param name="logger">Logger para registrar eventos y errores.</param>
    public AnalysisTextOpenAI(OpenIaConfig openIaConfig, ILogger<AnalysisTextOpenAI> logger)
    {
        _openIaConfig = openIaConfig ?? throw new ArgumentNullException(nameof(openIaConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Procesa una solicitud de análisis de texto de manera asíncrona.
    /// </summary>
    /// <param name="request">La solicitud de análisis de texto.</param>
    /// <returns>Una tarea que retorna un objeto AnalysisOpenIAResponse que contiene los resultados del análisis.</returns>
    public async Task<AnalysisOpenIAResponse> ProcessAsync(AnalysisOpenIARequest request)
    {
        var analysisResponse = new AnalysisOpenIAResponse() { Start = DateTime.Now };

        try
        {
            ValidateRequest(request);
            var result = await GetChatResultAsync(request);
            PopulateAnalysisResponse(result, analysisResponse);
        }
        catch (Exception ex)
        {
            HandleException(ex, analysisResponse);
        }

        analysisResponse.End = DateTime.Now;
        return analysisResponse;
    }

    /// <summary>
    /// Valida la solicitud de análisis para asegurar que el texto del prompt no esté vacío.
    /// </summary>
    /// <param name="request">La solicitud de análisis que se está validando.</param>
    private void ValidateRequest(AnalysisOpenIARequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PromptUser))
            throw new ArgumentException("Se ingresó un texto vacío");
    }

    /// <summary>
    /// Obtiene el resultado del chat utilizando la API de OpenAI basado en la solicitud proporcionada.
    /// </summary>
    /// <param name="request">La solicitud de análisis de texto.</param>
    /// <returns>Una tarea que retorna un resultado del chat.</returns>
    private async Task<ChatResult> GetChatResultAsync(AnalysisOpenIARequest request)
    {
        var api = new OpenAIAPI(_openIaConfig.ApiKey);
        return await CreateChatCompletionAsync(api, request);
    }

    /// <summary>
    /// Crea una finalización de chat en la API de OpenAI usando los detalles de la solicitud.
    /// </summary>
    /// <param name="api">La API de OpenAI utilizada para crear la finalización de chat.</param>
    /// <param name="request">La solicitud que incluye el modelo, temperatura, máximo de tokens y mensajes.</param>
    /// <returns>Una tarea que retorna el resultado del chat completado.</returns>
    private async Task<ChatResult> CreateChatCompletionAsync(OpenAIAPI api, AnalysisOpenIARequest request)
    {
        return await api.Chat.CreateChatCompletionAsync(new ChatRequest
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 1000,
            Messages = new ChatMessage[] {
                new ChatMessage(ChatMessageRole.System, request.PromptSystem),
                new ChatMessage(ChatMessageRole.User, request.PromptUser)
            }
        });
    }

    /// <summary>
    /// Llena la respuesta de análisis con los detalles obtenidos del resultado del chat.
    /// </summary>
    /// <param name="result">El resultado del chat obtenido.</param>
    /// <param name="response">La respuesta de análisis que se está poblado.</param>
    private void PopulateAnalysisResponse(ChatResult result, AnalysisOpenIAResponse response)
    {
        var openIaResponse = result.Choices[0].Message;
        if (openIaResponse == null || string.IsNullOrWhiteSpace(openIaResponse.TextContent))
            throw new InvalidOperationException("La respuesta de OPENIA fue nula o vacía");

        ValidateOpenIaResponse.Process(openIaResponse.TextContent);
        response.Success = true;
        response.Result = openIaResponse.TextContent;
    }

    /// <summary>
    /// Maneja las excepciones capturadas durante el procesamiento, registrando el error y actualizando la respuesta de análisis.
    /// </summary>
    /// <param name="ex">La excepción capturada durante el procesamiento.</param>
    /// <param name="response">La respuesta de análisis que se actualizará con los detalles del fallo.</param>
    private void HandleException(Exception ex, AnalysisOpenIAResponse response)
    {
        var message = ValidateOpenIaResponse.GetMessageOfBadRequestResponse(ex.Message);
        _logger.LogError(ex, ex.Message);
        response.Success = false;
        response.Result = message;
    }
}