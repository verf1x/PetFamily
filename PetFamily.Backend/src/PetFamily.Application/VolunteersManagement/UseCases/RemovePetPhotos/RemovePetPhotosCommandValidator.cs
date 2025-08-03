using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.UseCases.RemovePetPhotos;

public class RemovePetPhotosCommandValidator : AbstractValidator<RemovePetPhotosCommand>
{
    public RemovePetPhotosCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(RemovePetPhotosCommand.VolunteerId)));
        
        RuleFor(x => x.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(RemovePetPhotosCommand.PetId)));

        RuleFor(x => x.PhotoNames)
            .Must(x => x.Any())
            .WithError(Errors.General.ValueIsRequired(nameof(RemovePetPhotosCommand.PhotoNames)));
    }
}