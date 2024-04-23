namespace ADIA.OpenAi.Proxy.Models;

public record AnalysisOpenIARequest
{
    public string PromptSystem { get; set; }
    public string PromptUser { get; set; }
    public byte[] File { get; set; }
}