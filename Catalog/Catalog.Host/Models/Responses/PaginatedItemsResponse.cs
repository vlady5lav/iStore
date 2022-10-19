namespace Catalog.Host.Models.Responses;

public class PaginatedItemsResponse<T>
{
    public long Count { get; init; }

    public IEnumerable<T> Data { get; init; } = null!;

    public int PageIndex { get; init; }

    public int PageSize { get; init; }
}
