using ADIA.Shared.Enums;
using System.Text.Json.Serialization;

namespace ADIA.Model.DataTransfer.IaResponses;

public record AnalysisIAResultDto
{
    [JsonIgnore]
    public bool Success { get; set; }
    [JsonIgnore]
    public DateTime Start { get; set; }
    [JsonIgnore]
    public DateTime End { get; set; }

    public EntityEnums.DocumentType DocumentType { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }


    public static AnalysisIAResultDto CreateDefaultInvoice()
    {
        return new AnalysisIAResultDto()
        {
            DocumentType = EntityEnums.DocumentType.Invoice,
            Message = "El resultado devuelto es una FACTURA",
            Data = InvoiceIADto.CreateDefault()
        };
    } 

    public static AnalysisIAResultDto CreateDefaultGeneralText()
    {
        return new AnalysisIAResultDto()
        {
            DocumentType = EntityEnums.DocumentType.GeneralText,
            Message = "El resultado devuelto es un TEXTO GENERAL",
            Data = GeneralTextIADto.CreateDefault()
        };
    }

    public static AnalysisIAResultDto CreateDefaultUndefined()
    {
        return new AnalysisIAResultDto()
        {
            DocumentType = EntityEnums.DocumentType.Undefined,
            Message = "No se ha podido analizar el archivo",
            Data = UndefinedIADto.CreateDefault()
        };
    }

    public AnalysisIAResultDto CreateUndefined()
    {
        return new AnalysisIAResultDto()
        {
            Success = false,
            Start = Start,
            End = End,
            DocumentType = EntityEnums.DocumentType.Undefined,
            Data = Data,
            Message = Message,
        };
    }
}