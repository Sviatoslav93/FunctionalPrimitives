namespace WebApp.Shared;

public interface IPaginationRequest
{
    int Page { get; init; }

    int PageSize { get; init; }
}
