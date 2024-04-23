namespace ADIA.Model.DataTransfer.IaResponses;

public record UndefinedIADto
{
    public string Message { get; set; }

    public static UndefinedIADto CreateDefault() => new UndefinedIADto() { Message = string.Empty };
}