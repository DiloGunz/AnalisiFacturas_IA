namespace ADIA.Shared.Collection;

public class DataCollection<T>
{
    public static DataCollection<T> CreateInstance(int page = 1, int take = 50)
        => new DataCollection<T>()
        {
            Page = page,
            Take = take
        };

    public int Page { get; set; } = 1;
    public int Pages { get; set; }
    public int Take { get; set; }

    public int Total { get; set; }

    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    public bool HasItems => Items.Any();
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < Pages;

}