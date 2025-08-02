using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.Queries.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationHandlerDapper
    : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetFilteredPetsWithPaginationHandlerDapper(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> HandleAsync(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        var sqlQuery = new StringBuilder
        (
            "SELECT id, nickname, description, position, color, photos FROM pets"
        );

        var countQuery = new StringBuilder
        (
            "SELECT COUNT(*) FROM pets"
        );
        
        if (!string.IsNullOrWhiteSpace(query.Nickname))
        {
            const string filterQuery = "\nWHERE nickname = @Nickname";
            
            sqlQuery.Append(filterQuery);
            countQuery.Append(filterQuery);
            parameters.Add("Nickname", query.Nickname);
        }

        var totalCount = await connection.ExecuteScalarAsync<long>(
            countQuery.ToString(), parameters);
        
        sqlQuery.ApplySorting(query.SortBy, query.SortAscending);
        
        sqlQuery.ApplyPagination(parameters, query.PageNumber, query.PageSize);
        
        var pets = await connection.QueryAsync<PetDto, string, PetDto>(
            sqlQuery.ToString(),
            (pet, jsonFiles) =>
            {
                var files = JsonSerializer.Deserialize<PetFileDto[]>(jsonFiles) ?? [];

                pet.Photos = files;

                return pet;
            },
            splitOn: "photos",
            param: parameters);

        return new PagedList<PetDto>
        {
            Items = pets.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            PageNumber = query.PageNumber
        };
    }
}