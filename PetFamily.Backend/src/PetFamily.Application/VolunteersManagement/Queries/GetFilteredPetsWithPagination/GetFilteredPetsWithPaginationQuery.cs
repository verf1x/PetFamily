using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.Queries.GetFilteredPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    string? Nickname,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    bool? SortAscending,
    int PageNumber,
    int PageSize) : IQuery;