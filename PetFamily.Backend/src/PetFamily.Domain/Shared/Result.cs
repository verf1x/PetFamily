namespace PetFamily.Domain.Shared;

public class Result
{
    public string? Error { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;

    
    public Result(bool isSuccess, string? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException();

        if (!isSuccess && error is null)
            throw new InvalidOperationException();
        
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success() => new(true, null);
    
    public static implicit operator Result(string error) => new(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue _value;
    
    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

    public Result(TValue value, bool isSuccess, string? error) : base(isSuccess, error)
    {
        _value = value;
    }
    
    public static Result<TValue> Success(TValue value) => new(value, true, null);
    
    public static Result<TValue> Failure(string error) => new(default!, false, error);
    
    public static implicit operator Result<TValue>(TValue value) => new(value, true, null);
    
    public static implicit operator Result<TValue>(string error) => new(default!, false, error);
}