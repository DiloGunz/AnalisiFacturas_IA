using ADIA.Shared.Enums;

namespace ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;

public record AnalysisResponseListDto
{
    public DateTime StartAnalysis { get; set; }
    public DateTime EndAnalysis { get; set; }
    public bool IsSuccess { get; set; }
    public decimal ResponseTime { get; set; }
    public string Message { get; set; }
    public EntityEnums.DocumentType DocumentType { get; set; }
    public EntityEnums.Ia Ia { get; set; }
}