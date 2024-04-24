using ADIA.OpenAi.Proxy.Config.Models;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Utils;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace ADIA.OpenAi.Proxy.Services;

/// <summary>
/// Clase para analizar el texto obtenido de azure IA
/// </summary>
public class AnalysisTextOpenAI : IAnalysisTextOpenAI
{
    private readonly OpenIaConfig _openIaConfig;
    private readonly ILogger<AnalysisTextOpenAI> _logger;

    public AnalysisTextOpenAI(OpenIaConfig openIaConfig, ILogger<AnalysisTextOpenAI> logger)
    {
        _openIaConfig = openIaConfig ?? throw new ArgumentNullException(nameof(openIaConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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

    private void ValidateRequest(AnalysisOpenIARequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PromptUser))
            throw new ArgumentException("Se ingresó un texto vacío");
    }

    private async Task<ChatResult> GetChatResultAsync(AnalysisOpenIARequest request)
    {
        var api = new OpenAIAPI(_openIaConfig.ApiKey);
        return await CreateChatCompletionAsync(api, request);
    }

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

    private void PopulateAnalysisResponse(ChatResult result, AnalysisOpenIAResponse response)
    {
        var openIaResponse = result.Choices[0].Message;
        if (openIaResponse == null || string.IsNullOrWhiteSpace(openIaResponse.TextContent))
            throw new InvalidOperationException("La respuesta de OPENIA fue nula o vacía");

        ValidateOpenIaResponse.Process(openIaResponse.TextContent);
        response.Success = true;
        response.Result = openIaResponse.TextContent;
    }

    private void HandleException(Exception ex, AnalysisOpenIAResponse response)
    {
        var message = ValidateOpenIaResponse.GetMessageOfBadRequestResponse(ex.Message);
        _logger.LogError(ex, ex.Message);
        response.Success = false;
        response.Result = message;
    }
}