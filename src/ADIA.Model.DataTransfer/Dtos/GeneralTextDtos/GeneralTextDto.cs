namespace ADIA.Model.DataTransfer.Dtos.GeneralTextDtos;

public record GeneralTextDto
{
    public long Id { get; set; }
    public string Description { get; set; }
    public string Summary { get; set; }
    public string Sentiment { get; set; }
}