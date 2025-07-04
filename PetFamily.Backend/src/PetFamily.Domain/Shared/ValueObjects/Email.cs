using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public partial record Email
{
    public string Value { get; } = null!;

    private Email(string value) => Value = value;
    
    public static Result<Email, Error> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !EmailRegex().IsMatch(email))
            return Errors.General.ValueIsInvalid(nameof(email));

        return new Email(email);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}