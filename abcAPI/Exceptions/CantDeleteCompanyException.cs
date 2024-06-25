namespace abcAPI.Exceptions;

public class CantDeleteCompanyException : Exception
{
    public CantDeleteCompanyException(string message) : base(message)
    {
    }
}