namespace Catalog.Host.Data;

public class PaginatedItems<T>
{
    public IEnumerable<T?> Data { get; init; } = null!;

    public long TotalCount { get; init; }
}
