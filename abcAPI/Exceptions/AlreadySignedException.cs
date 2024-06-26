namespace abcAPI.Exceptions;

public class AlreadySignedException : Exception
{
    public AlreadySignedException(string message) : base(message)
    {
    }
}