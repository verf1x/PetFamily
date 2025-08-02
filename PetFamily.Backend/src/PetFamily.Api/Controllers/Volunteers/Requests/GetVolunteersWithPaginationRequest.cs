using PetFamily.Application.VolunteersManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.Api.Controllers.Volunteers.Requests;

public record GetVolunteersWithPaginationRequest(int PageNumber, int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery()
        => new (PageNumber, PageSize);
}