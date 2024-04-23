using ADIA.OpenAi.Proxy.Models;

namespace ADIA.OpenAi.Proxy.Services;

public interface IAnalysisTextOpenAI
{
    Task<AnalysisOpenIAResponse> ProcessAsync(AnalysisOpenIARequest request);
}