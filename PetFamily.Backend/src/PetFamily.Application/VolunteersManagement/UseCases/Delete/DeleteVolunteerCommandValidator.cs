using FluentValidation;

namespace PetFamily.Application.VolunteersManagement.UseCases.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(r => r.VolunteerId).NotEmpty();
    }
}