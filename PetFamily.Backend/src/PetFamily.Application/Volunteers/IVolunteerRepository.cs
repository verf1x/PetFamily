using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteers.Entities;

namespace PetFamily.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Guid> SaveAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Guid> DeleteAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer>> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    
    Task<Result<Volunteer>> GetByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default);
}