using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.UseCases.MovePet;

public class MovePetCommandValidator : AbstractValidator<MovePetCommand>
{
    public MovePetCommandValidator()
    {
        RuleFor(mp => mp.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(MovePetCommand.VolunteerId)));

        RuleFor(mp => mp.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(MovePetCommand.PetId)));

        RuleFor(mp => mp.NewPosition)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid(nameof(MovePetCommand.NewPosition)));
    }
}