using ADIA.OpenAi.Proxy.Config.Models;
using ADIA.OpenAi.Proxy.Models;
using ADIA.OpenAi.Proxy.Utils;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Models;
using System.Diagnostics.Contracts;
using static OpenAI_API.Chat.ChatMessage;

namespace ADIA.OpenAi.Proxy.Services;

public class AnalysisImageOpenAIService : IAnalysisImageOpenAIService
{
    private readonly OpenIaConfig _openIaConfig;
    private readonly ILogger<AnalysisImageOpenAIService> _logger;

    public AnalysisImageOpenAIService(OpenIaConfig openIaConfig, ILogger<AnalysisImageOpenAIService> logger)
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
            var api = new OpenAIAPI(_openIaConfig.ApiKey);
            var response = await PerformImageAnalysis(api, request);
            ProcessResponse(response, analysisResponse);
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
        if (request.File == null || request.File.Length == 0)
            throw new ArgumentException("Se ingresó una imagen inválida");
    }

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

    private void ProcessResponse(string response, AnalysisOpenIAResponse analysisResponse)
    {
        ValidateOpenIaResponse.Process(response);
        analysisResponse.Success = true;
        analysisResponse.Result = FixResponse.Process(response);
    }

    public void HandleException(Exception ex, AnalysisOpenIAResponse analysisResponse)
    {
        var message = ValidateOpenIaResponse.GetMessageOfBadRequestResponse(ex.Message);
        _logger.LogError(ex, ex.Message);
        analysisResponse.Success = false;
        analysisResponse.Result = message;
    }
}