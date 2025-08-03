using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.VolunteersManagement.UseCases.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(fn => FullName.Create(fn.FirstName, fn.LastName, fn.MiddleName!));
        
        RuleFor(r => r.Email)
            .MustBeValueObject(Email.Create);

        RuleFor(r => r.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(r => r.ExperienceYears)
            .MustBeValueObject(Experience.Create);
        
        RuleFor(r => r.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleForEach(r => r.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Url));

        RuleForEach(r => r.HelpRequisites)
            .MustBeValueObject(hr => HelpRequisite.Create(hr.Name, hr.Description));
    }
}