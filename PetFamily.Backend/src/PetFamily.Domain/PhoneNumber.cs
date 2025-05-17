using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain;

public class PhoneNumber : ValueObject
{
    public string Number { get; }
    
    private PhoneNumber(string number)
    {
        Number = number;
    }

    public static Result<PhoneNumber> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) ||
            !Regex.IsMatch(phoneNumber, @"^\+?\d{1,4}?[\s-]?(\(?\d{1,5}\)?[\s-]?)?[\d\s-]{5,15}$"))
        {
            return Result.Failure<PhoneNumber>("Invalid phone number");
        }
        
        PhoneNumber number = new(phoneNumber);
        
        return Result.Success(number);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}