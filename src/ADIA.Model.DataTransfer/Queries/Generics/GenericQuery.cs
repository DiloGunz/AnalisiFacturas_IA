using ADIA.Shared.Extensions;

namespace ADIA.Model.DataTransfer.Queries.Generics;

public record GenericQuery
{
    private string? searchString = "";

    public string SearchString
    {
        get => (searchString ?? "").ToUpperTrim();
        set => searchString = value;
    }
}