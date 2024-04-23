using ADIA.AzureIa.Proxy.Config.Models;
using ADIA.AzureIa.Proxy.Responses;
using ADIA.OpenAi.Proxy.Responses;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;

namespace ADIA.AzureIa.Proxy.Services;

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
            var textResults = await AnalyzeTextWithComputerVisionAsync(request.File);
            if (!textResults.Any())
            {
                throw new InvalidOperationException("Azure IA no pudo analizar el archivo PDF.");
            }
            response.Success = true;
            response.Result = string.Join("\n", textResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PDF with Azure IA.");
            response.Success = false;
            response.Result = ex.Message;
        }
        response.End = DateTime.Now;
        return response;
    }

    private async Task<string> PerformAnalysisAsync(byte[] pdfFile)
    {
        var textResults = await AnalyzeTextWithComputerVisionAsync(pdfFile);
        if (!textResults.Any())
        {
            throw new InvalidOperationException("No text found in PDF.");
        }
        return string.Join("\n", textResults);
    }

    private async Task<List<string>> AnalyzeTextWithComputerVisionAsync(byte[] fileData)
    {
        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_azureIaConfig.ApiKey))
        {
            Endpoint = _azureIaConfig.EndPoint
        };
        var textHeaders = await client.ReadInStreamAsync(new MemoryStream(fileData));
        var operationId = ExtractOperationId(textHeaders.OperationLocation);
        return await WaitForTextAnalysisResult(client, operationId);
    }

    private string ExtractOperationId(string operationLocation)
    {
        return operationLocation.Substring(operationLocation.LastIndexOf('/') + 1);
    }

    private async Task<List<string>> WaitForTextAnalysisResult(ComputerVisionClient client, string operationId)
    {
        ReadOperationResult results;
        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);
        return results.AnalyzeResult.ReadResults.SelectMany(r => r.Lines).Select(l => l.Text).ToList();
    }
}