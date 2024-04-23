namespace ADIA.OpenAi.Proxy.Models;

public record AnalysisOpenIAResponse
{
    public bool Success { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Result { get; set; }
}