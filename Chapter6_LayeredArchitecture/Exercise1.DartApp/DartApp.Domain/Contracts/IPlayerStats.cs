namespace DartApp.Domain.Contracts
{
    public interface IPlayerStats
    {
        double AverageThrow { get;  }
        int Total180 { get; }
        int BestThrow { get; }
        double AverageBestThrow { get; }
    }
}
