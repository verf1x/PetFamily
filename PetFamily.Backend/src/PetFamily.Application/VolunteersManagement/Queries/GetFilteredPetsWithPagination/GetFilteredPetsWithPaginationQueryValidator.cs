using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.Queries.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationQueryValidator
    : AbstractValidator<GetFilteredPetsWithPaginationQuery>
{
    public GetFilteredPetsWithPaginationQueryValidator()
    {
        RuleFor(fp => fp.Nickname)
            .MinimumLength(1)
            .When(fp => !string.IsNullOrWhiteSpace(fp.Nickname));

        RuleFor(fp => fp.PositionFrom)
            .GreaterThanOrEqualTo(1)
            .When(fp => fp.PositionFrom.HasValue);

        RuleFor(fp => fp.PositionTo)
            .GreaterThanOrEqualTo(1)
            .When(fp => fp.PositionTo.HasValue)
            .WithError(Errors.General.ValueIsInvalid(nameof(GetFilteredPetsWithPaginationQuery.PositionTo)));

        RuleFor(fp => fp)
            .Must(pf => !pf.PositionFrom.HasValue || !pf.PositionTo.HasValue || 
                        pf.PositionFrom <= pf.PositionTo)
            .WithError(Error.Validation(
                "invalid.position.range",
                "PositionFrom must be less than or equal to PositionTo.",
                "Position range"));
        
        RuleFor(fp => fp.PageSize)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid(nameof(GetFilteredPetsWithPaginationQuery.PageSize)));
        
        RuleFor(fp => fp.PageNumber)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid(nameof(GetFilteredPetsWithPaginationQuery.PageNumber)));

        RuleFor(fp => fp.SortBy)
            .Must(sb => sb?.Length > 0)
            .WithError(Errors.General.ValueIsInvalid(nameof(GetFilteredPetsWithPaginationQuery.SortBy)));
    }
}