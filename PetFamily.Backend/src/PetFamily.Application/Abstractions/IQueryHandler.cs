using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    Task<Result<TResponse, ErrorList>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default);
}

public interface IQueryHandler<TResponse>
{
    Task<Result<TResponse, ErrorList>> HandleAsync(
        CancellationToken cancellationToken = default);
}