using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(int PageNumber, int PageSize) : IQuery;