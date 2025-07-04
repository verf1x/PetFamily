using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record PhoneNumber
{
    public string Value { get; }
    
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber, Error> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) ||
            !Regex.IsMatch(phoneNumber, @"^\+?\d{1,4}?[\s-]?(\(?\d{1,5}\)?[\s-]?)?[\d\s-]{5,15}$"))
        {
            return Errors.General.ValueIsInvalid(nameof(phoneNumber));
        }

        return new PhoneNumber(phoneNumber);
    }
}