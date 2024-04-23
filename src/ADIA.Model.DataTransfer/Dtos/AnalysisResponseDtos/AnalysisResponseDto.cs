using ADIA.Model.DataTransfer.Dtos.GeneralTextDtos;
using ADIA.Model.DataTransfer.Dtos.InvoiceDtos;
using ADIA.Model.DataTransfer.IaResponses;
using ADIA.Shared.Enums;

namespace ADIA.Model.DataTransfer.Dtos.AnalysisResponseDtos;

public record AnalysisResponseDto
{
    public long Id { get; set; }
    public DateTime StartAnalysis { get; set; }
    public DateTime EndAnalysis { get; set; }
    public bool IsSuccess { get; set; }
    public int ResponseTime { get; set; }
    public string Message { get; set; }
    public EntityEnums.DocumentType DocumentType { get; set; }
    public EntityEnums.Ia Ia { get; set; }


    public InvoiceDto Invoice { get; set; }
    public GeneralTextDto GeneralText { get; set; }
    public UndefinedDto UndefinedDto { get; set; }
}