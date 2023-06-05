namespace Common.Validation;

public class ValidationException : Exception
{
    public ValidationException(string exceptionMessage) : base(exceptionMessage)
    {
        
    }
}