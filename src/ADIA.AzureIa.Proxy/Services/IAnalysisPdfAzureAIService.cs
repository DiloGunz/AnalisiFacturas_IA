using ADIA.AzureIa.Proxy.Responses;
using ADIA.OpenAi.Proxy.Responses;

namespace ADIA.AzureIa.Proxy.Services;


public interface IAnalysisPdfAzureAIService
{
    Task<AnalysisAzureIAResponse> ProcessPdfAsync(AnalysisAzureIARequest request);
}