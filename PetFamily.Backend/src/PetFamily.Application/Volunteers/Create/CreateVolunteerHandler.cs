using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteersManagement.Entities;
using PetFamily.Domain.VolunteersManagement.ValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<CreateVolunteerCommand> validator,
        IApplicationDbContext dbContext,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var email = Email.Create(command.Email).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        var volunteerByEmail = 
            await _volunteersRepository.GetByEmailAsync(email, cancellationToken);
        
        var volunteerByPhoneNumber = 
            await _volunteersRepository.GetByPhoneNumberAsync(phoneNumber, cancellationToken);
        
        if (volunteerByEmail.IsSuccess || volunteerByPhoneNumber.IsSuccess)
            return Errors.Module.AlreadyExists().ToErrorList();
        
        var id = VolunteerId.CreateNew();
        
        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.MiddleName!).Value;
        
        var description = Description.Create(command.Description).Value;
        var experience = Experience.Create(command.ExperienceYears).Value;
        
        var socialNetworks = new ValueObjectList<SocialNetwork>(
            command.SocialNetworks
                .Select(s => SocialNetwork.Create(s.Name, s.Url).Value)
                .ToList());
        
        var helpRequisites = new ValueObjectList<HelpRequisite>(
            command.HelpRequisites
                .Select(r => HelpRequisite.Create(r.Name, r.Description).Value)
                .ToList());
        
        Volunteer volunteer = new(
            id,
            fullName, 
            email,
            description,
            experience,
            phoneNumber,
            socialNetworks, 
            helpRequisites);
        
        var result = await _volunteersRepository.AddAsync(volunteer, cancellationToken);
        
        _logger.LogInformation("Created volunteer with ID: {id}", id);

        return result;
    }
}