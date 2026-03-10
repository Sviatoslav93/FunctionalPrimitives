namespace WebApp.Shared;

public interface IOrderingRequest
{
    string? OrderBy { get; init; }

    bool OrderDescending { get; init; }
}

public class PagedResponse<T>
{
    public required IReadOnlyList<T> Items { get; init; }

    public required int Page { get; init; }

    public required int PageSize { get; init; }

    public required int Count { get; init; }
}
