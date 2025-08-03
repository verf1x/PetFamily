using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteersManagement.Queries.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationHandler
    : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetFilteredPetsWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> HandleAsync(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var petsQuery = _readDbContext.Pets;

        Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "nickname" => p => p.Nickname,
            "position" => p => p.Position,
            "color" => p => p.Color,
            _ => p => p.Id
        };

        petsQuery = query.SortAscending != null && query.SortAscending.Value
            ? petsQuery.OrderBy(keySelector)
            : petsQuery.OrderByDescending(keySelector);

        petsQuery = petsQuery.WhereIf(
            !string.IsNullOrEmpty(query.Nickname),
            p => p.Nickname.Contains(query.Nickname!));

        petsQuery = petsQuery
            .WhereIf(query.PositionTo is not null,
                p => p.Position <= query.PositionTo!.Value);

        petsQuery = petsQuery
            .WhereIf(query.PositionFrom is not null,
                p => p.Position >= query.PositionFrom!.Value);

        return await petsQuery
            .ToPagedList(query.PageNumber, query.PageSize, cancellationToken);
    }
}