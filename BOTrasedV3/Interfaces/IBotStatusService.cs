using BOTrasedV3.Models;

namespace BOTrasedV3.Interfaces
{
    public interface IBotStatusService
    {
        Task<BotStatus> GetRandomStatus();
    }
}