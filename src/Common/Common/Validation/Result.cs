namespace Common.Validation;

public class Result
{
    private List<string> _messages = new();

    public static Result Ok()
    {
        return new Result();
    }

    public static Result Fail(string message)
    {
        var result = new Result();
        result.AddError(message);
        return result;
    }
    
    public static Result<TResult> Ok<TResult>(TResult value) where TResult : class
    {
        return new Result<TResult>(){Value = value};
    }

    public static Result<TResult> Fail<TResult>(string message) where TResult : class
    {
        var result = new Result<TResult>();
        result.AddError(message);
        return result;
    }

    public List<string> GetErrors()
    {
        return _messages;
    }

    public Result AddError(string message)
    {
        _messages ??= new List<string>();

        _messages.Add(message);
        return this;
    }

    public Result Combine(Result result)
    {
        _messages.AddRange(result.GetErrors());
        return this;
    }
    
    public Result Combine<T>(Result<T> result) where T : class
    {
        _messages.AddRange(result.GetErrors());
        return this;
    }

    public void ValidateAndThrow()
    {
        if (_messages == null || !_messages.Any())
            return;

        throw new ValidationException(string.Join("\n", _messages));
    }

    public bool IsSuccess() => _messages == null || !_messages.Any();
    public bool IsFail() => _messages != null && _messages.Any();
}

public class Result<T> : Result where T : class
{
    public T Value { get; set; }
}