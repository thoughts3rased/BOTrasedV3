namespace BOTrasedV3.Interfaces
{
    public interface ICommandStatisticsService
    {
        Task LogCommandUsage(string commandName);
    }
}
