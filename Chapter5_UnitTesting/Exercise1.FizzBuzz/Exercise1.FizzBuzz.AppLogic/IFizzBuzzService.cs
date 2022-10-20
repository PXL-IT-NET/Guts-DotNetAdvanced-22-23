namespace Exercise1.FizzBuzz.AppLogic;

public interface IFizzBuzzService
{
    string GenerateFizzBuzzText(int fizzFactor, int buzzFactor, int lastNumber);
}