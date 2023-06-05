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

    public IEnumerable<string> GetMessages()
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
        _messages.AddRange(result.GetMessages());
        return this;
    }

    public void ValidateAndThrow()
    {
        if (_messages == null || !_messages.Any())
            return;

        throw new ValidationException(string.Join("\n", _messages));
    }
    public bool IsSuccess() =>  _messages == null || !_messages.Any();
    public bool IsFail() => _messages != null && _messages.Any();
}




public class Result<T> where T : class
{
    private List<string> _messages = new() ;
    public T Value;

    public static Result<T> Ok(T entity)
    {
        return new Result<T> {Value = entity};
    }
    
    public static Result<T> Fail(string message)
    {
        var result = new Result<T>();
        result.AddError(message);
        return result;
    }
    
    private IEnumerable<string> GetMessages()
    {
        return _messages;
    }
    
    public Result<T> AddError(string message)
    {
        _messages ??= new List<string>();
        
        _messages.Add(message);
        return this;
    }
    
    public Result<T> Combine(Result<T> result)
    {
        _messages.AddRange(result.GetMessages());
        return this;
    }
    
    public Result<T> Combine(Result result)
    {
        _messages.AddRange(result.GetMessages());
        return this;
    }
    
    public void ValidateAndThrow()
    {
        if (_messages == null || !_messages.Any())
            return;

        throw new ValidationException(string.Join("\n", _messages));
    }
    
    public bool IsSuccess() =>  _messages == null || !_messages.Any();
    public bool IsFail() => _messages != null && _messages.Any();
}

