using ADIA.AzureIa.Proxy.Config.Models;
using ADIA.AzureIa.Proxy.Responses;
using ADIA.OpenAi.Proxy.Responses;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;

namespace ADIA.AzureIa.Proxy.Services;

/// <summary>
/// Clase para extraer el texto de un archivo pdf
/// </summary>
public class AnalysisPdfAzureAIService : IAnalysisPdfAzureAIService
{
    private readonly AzureIaConfig _azureIaConfig;
    private readonly ILogger<AnalysisPdfAzureAIService> _logger;

    public AnalysisPdfAzureAIService(AzureIaConfig azureIaConfig, ILogger<AnalysisPdfAzureAIService> logger)
    {
        _azureIaConfig = azureIaConfig ?? throw new ArgumentNullException(nameof(azureIaConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AnalysisAzureIAResponse> ProcessPdfAsync(AnalysisAzureIARequest request)
    {
        var response = new AnalysisAzureIAResponse { Start = DateTime.Now };
        try
        {
            var textResults = await ExtractTextFromPdfAsync(request.File);
            ValidateResults(textResults);
            response.Result = string.Join("\n", textResults);
            response.Success = true;
        }
        catch (Exception ex)
        {
            HandleException(ex, response);
        }
        response.End = DateTime.Now;
        return response;
    }

    private async Task<List<string>> ExtractTextFromPdfAsync(byte[] fileData)
    {
        var client = CreateComputerVisionClient();
        var operationId = await SubmitReadRequestAsync(client, fileData);
        return await GetTextResultsAsync(client, operationId);
    }

    private ComputerVisionClient CreateComputerVisionClient()
    {
        return new ComputerVisionClient(new ApiKeyServiceClientCredentials(_azureIaConfig.ApiKey))
        {
            Endpoint = _azureIaConfig.EndPoint
        };
    }

    private async Task<string> SubmitReadRequestAsync(ComputerVisionClient client, byte[] fileData)
    {
        var textHeaders = await client.ReadInStreamAsync(new MemoryStream(fileData));
        return ExtractOperationId(textHeaders.OperationLocation);
    }

    private string ExtractOperationId(string operationLocation)
    {
        return operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);
    }

    private async Task<List<string>> GetTextResultsAsync(ComputerVisionClient client, string operationId)
    {
        ReadOperationResult results;
        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

        return results.AnalyzeResult.ReadResults.SelectMany(r => r.Lines).Select(l => l.Text).ToList();
    }

    private void ValidateResults(List<string> results)
    {
        if (!results.Any())
            throw new InvalidOperationException("Azure IA no pudo analizar el archivo PDF.");
    }

    private void HandleException(Exception ex, AnalysisAzureIAResponse response)
    {
        _logger.LogError(ex, "Error processing PDF with Azure IA.");
        response.Success = false;
        response.Result = ex.Message;
    }
}