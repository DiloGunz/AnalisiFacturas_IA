using ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;
using ADIA.Shared.Enums;

namespace ADIA.Model.DataTransfer.Dtos.AnalysisDtos;

public record AnalysisListDto
{
    public long Id { get; set; }
    public DateTime AnalysisDate { get; set; }
    public string FileName { get; set; } = null!;
    public string? FileBase64 { get; set; }
    public string FileExtension { get; set; } = null!;
    public EntityEnums.FileType FileType { get; set; }

    public AnalysisResponseListDto AnalysisResponseListDto { get; set; } = new AnalysisResponseListDto();
}