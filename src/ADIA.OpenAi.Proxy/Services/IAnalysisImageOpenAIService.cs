using ADIA.OpenAi.Proxy.Models;

namespace ADIA.OpenAi.Proxy.Services;

public interface IAnalysisImageOpenAIService
{
    Task<AnalysisOpenIAResponse> ProcessAsync(AnalysisOpenIARequest request);
}