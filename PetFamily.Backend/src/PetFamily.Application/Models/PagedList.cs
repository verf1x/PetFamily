namespace PetFamily.Application.Models;

public class PagedList<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    
    public required long TotalCount { get; init; }
    
    public required int PageSize { get; init; }
    
    public required int PageNumber { get; init; }
    
    public bool HasNextPage => PageNumber * PageSize < TotalCount;
    
    public bool HasPreviousPage => PageNumber > 1;
}