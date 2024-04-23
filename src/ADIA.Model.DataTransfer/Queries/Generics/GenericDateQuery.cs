namespace ADIA.Model.DataTransfer.Queries.Generics;

public record GenericDateQuery : GenericQuery
{
    public DateTime DtFrom { get; set; } = DateTime.Now.AddDays(-10);
    public DateTime DtTo { get; set; } = DateTime.Now;
}