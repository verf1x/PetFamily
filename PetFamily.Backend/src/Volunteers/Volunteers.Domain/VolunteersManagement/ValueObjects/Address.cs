using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Framework;

namespace PetFamily.Volunteers.Domain.VolunteersManagement.ValueObjects;

public class Address : ComparableValueObject
{
    public IReadOnlyList<string> AddressLines { get; }

    public string Locality { get; }

    public string? Region { get; }

    public string? PostalCode { get; }

    public string CountryCode { get; }

    private Address(
        IReadOnlyList<string> addressLines,
        string locality,
        string? region,
        string? postalCode,
        string countryCode)
    {
        AddressLines = addressLines;
        Locality = locality;
        Region = region;
        PostalCode = postalCode;
        CountryCode = countryCode;
    }

    public static Result<Address, Error> Create(
        IList<string> addressLines,
        string locality,
        string? region,
        string? postalCode,
        string countryCode)
    {
        if (addressLines.Count is < Constants.MinAddressLines or > Constants.MaxAddressLines)
            return Errors.General.ValueIsInvalid(nameof(addressLines));

        if (string.IsNullOrWhiteSpace(locality))
            return Errors.General.ValueIsRequired(nameof(locality));

        if (string.IsNullOrWhiteSpace(countryCode) || !Regex.IsMatch(countryCode, @"^[A-Z]{2}$"))
            return Errors.General.ValueIsRequired(nameof(countryCode));

        return new Address(addressLines.AsReadOnly(), locality, region, postalCode, countryCode);
    }

    public override string ToString()
    {
        List<string> parts = [];
        parts.AddRange(AddressLines);
        parts.Add(Locality);

        if (!string.IsNullOrEmpty(Region))
            parts.Add(Region!);

        if (!string.IsNullOrEmpty(PostalCode))
            parts.Add(PostalCode!);

        parts.Add(CountryCode);
        return string.Join(", ", parts);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return ToString();
    }
}