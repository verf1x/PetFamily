using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteers.ValueObjects;

public record SocialNetwork
{
    public string Name { get; }
    public string Url { get; }

    private SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public static Result<SocialNetwork, Error> Create(string name, string url)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsRequired(nameof(name));
        
        if(string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            return Errors.General.ValueIsInvalid(nameof(url));
        
        return new SocialNetwork(name, url);
    }
}