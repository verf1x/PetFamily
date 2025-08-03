using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.VolunteersManagement.UseCases.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(c => c.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired(nameof(AddPetCommand.VolunteerId)));

        RuleFor(c => c.Nickname)
            .MustBeValueObject(Nickname.Create);

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.Color)
            .MustBeValueObject(Color.Create);

        RuleFor(c => c.HealthInfoDto)
            .Must(cb => !string.IsNullOrWhiteSpace(cb.HealthStatus));

        RuleFor(c => c.AddressDto)
            .MustBeValueObject(c =>
                Address.Create(
                    c.AddressLines.ToList(),
                    c.Locality,
                    c.Region,
                    c.PostalCode,
                    c.CountryCode));

        RuleFor(c => c.MeasurementsDto)
            .MustBeValueObject(c => Measurements.Create(c.Height, c.Weight));

        RuleFor(c => c.OwnerPhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(c => c.DateOfBirth)
            .NotEmpty();

        RuleForEach(c => c.HelpRequisites)
            .NotNull();

        RuleForEach(c => c.Photos)
            .Must(cb => cb.Content.Length != 0)
            .MustBeValueObject(cb => FilePath.Create(cb.FileName));
    }
}