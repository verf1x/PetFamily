namespace PetFamily.Api.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    public object? Result { get; }
    
    public IReadOnlyList<ResponseError> Errors { get; }

    public DateTime CreationDate { get; }

    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        CreationDate = DateTime.Now;
    }

    public static Envelope Ok(object? result = null) 
        => new(result, []);

    public static Envelope Error(IEnumerable<ResponseError> errors)
        => new(null, errors);
}