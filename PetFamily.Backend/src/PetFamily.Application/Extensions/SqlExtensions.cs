using System.Text;
using Dapper;

namespace PetFamily.Application.Extensions;

public static class SqlExtensions
{
    public static void ApplySorting(
        this StringBuilder sqlBuilder,
        string? sortBy,
        bool? sortAscending)
    {
        if(string.IsNullOrWhiteSpace(sortBy) || sortAscending is null)
            return;
        
        var sortDirection = sortAscending.Value ? "ASC" : "DESC";
        
        sqlBuilder.Append($"\nORDER BY {sortBy} {sortDirection}");
    }
    
    public static void ApplyPagination(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        int pageNumber,
        int pageSize)
    {
        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", (pageNumber - 1) * pageSize);
        sqlBuilder.Append("\nLIMIT @PageSize OFFSET @Offset");
    }
}