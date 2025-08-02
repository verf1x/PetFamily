using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.UseCases.UploadPetPhotos;

public class UploadPetPhotosCommandValidator : AbstractValidator<UploadPetPhotosCommand>
{
    public UploadPetPhotosCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(UploadPetPhotosCommand.VolunteerId)));
        
        RuleFor(x => x.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(UploadPetPhotosCommand.PetId)));

        RuleFor(x => x.Photos)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(UploadPetPhotosCommand.Photos)));
    }
}