using RecipeManagementSystem.Shared.Errors;

namespace RecipeManagementSystem.Shared.Models;

public class Result
{
    public bool IsSuccess { get; }
    
    public Error Error { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Successful result cannot have an error message.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Failure result must have an error message.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result() : base(true, Error.None)
    {
        
    }
    
    private Result(bool isSucess) : base(isSucess, Error.None)
    {
        Value = default!;
    }
    
    private Result(T value) : base(true, Error.None)
    {
        Value = value;
    }

    private Result(Error error) : base(false, error)
    {
        Value = default!;
    }
    
    private Result(T value, Error error) : base(false, error)
    {
        Value = value;
    }

    public static Result<T> Success() => new();
    
    public static Result<T> Success(T value) => new(value);
    
    public static new Result<T> Failure() => new(false);

    public static new Result<T> Failure(Error error) => new(error);
    
    public static new Result<T> Failure(T value, Error error) => new(value, error);
}