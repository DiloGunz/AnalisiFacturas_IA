namespace ADIA.Model.DataTransfer.IaResponses;

public record GeneralTextIADto
{
    public string Description { get; set; }
    public string Summary { get; set; }
    public string Sentiment { get; set; }

    public static GeneralTextIADto CreateDefault()
    {
        return new GeneralTextIADto()
        {
            Description = string.Empty,
            Sentiment = string.Empty,
            Summary = string.Empty
        };
    }
}