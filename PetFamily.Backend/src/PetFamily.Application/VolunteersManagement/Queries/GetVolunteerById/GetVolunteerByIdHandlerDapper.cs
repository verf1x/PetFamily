using CSharpFunctionalExtensions;
using Dapper;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.Queries.GetVolunteerById;

public class GetVolunteerByIdHandlerDapper : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetVolunteerByIdHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<VolunteerDto, ErrorList>> HandleAsync(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        const string sqlQuery =
            """
            SELECT id, description, total_years, phone_number from volunteers
            WHERE id = @VolunteerId
            """;

        var parameters = new DynamicParameters();
        parameters.Add("VolunteerId", query.VolunteerId);

        var volunteerDto = await connection.QuerySingleOrDefaultAsync<VolunteerDto>(
            sqlQuery,
            parameters);

        if (volunteerDto is null)
            return Errors.General.NotFound(query.VolunteerId).ToErrorList();

        return volunteerDto;
    }
}