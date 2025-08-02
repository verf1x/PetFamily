using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid", label);
        }
        
        public static Error NotFound(Guid? id = null)
        {
            var forId = id is null ? "" : $" for id '{id}'";
            return Error.NotFound("record.not.found", $"record not found{forId}");
        }
        
        public static Error ValueIsRequired(string? name = null)
        {
            var label = name is null ? " " : " " + name + " ";
            return Error.NotFound("value.is.required", $"invalid{label}length");
        }

        public static Error Conflict(Guid? id = null)
        {
            var forId = id is null ? "" : $" for id '{id}'";
            return Error.NotFound("value.already.exists", $"value already exists{forId}");
        }
    }

    public static class Volunteer
    {
        public static Error AlreadyExists()
        {
            return Error.Validation("record.already.exists", $"Volunteer already exists", null);
        }
    }
}