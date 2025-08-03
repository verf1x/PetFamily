using PetFamily.Application.VolunteersManagement.Queries.GetFilteredPetsWithPagination;

namespace PetFamily.Api.Controllers.Pets.Requests;

public record GetFilteredPetsWithPaginationRequest(
    string? Nickname,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    bool? SortAscending,
    int Page,
    int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() 
        => new(
            Nickname,
            PositionFrom,
            PositionTo,
            SortBy,
            SortAscending,
            Page,
            PageSize);
}