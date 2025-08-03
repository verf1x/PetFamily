using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationQueryValidator 
    : AbstractValidator<GetVolunteersWithPaginationQuery>
{
    public GetVolunteersWithPaginationQueryValidator()
    {
        RuleFor(vq => vq.PageSize)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid(nameof(GetVolunteersWithPaginationQuery.PageSize)));
        
        RuleFor(vq => vq.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid(nameof(GetVolunteersWithPaginationQuery.PageNumber)));
    }
}