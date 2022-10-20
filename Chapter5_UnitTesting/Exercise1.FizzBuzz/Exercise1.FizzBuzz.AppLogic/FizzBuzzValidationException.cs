namespace Exercise1.FizzBuzz.AppLogic;

public class FizzBuzzValidationException : ApplicationException
{
    public FizzBuzzValidationException(string message) : base(message)
    {
    }
}