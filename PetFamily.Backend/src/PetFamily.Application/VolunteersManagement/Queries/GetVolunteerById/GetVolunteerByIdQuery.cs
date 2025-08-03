using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteersManagement.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;