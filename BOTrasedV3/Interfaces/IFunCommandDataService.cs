using BOTrasedV3.Models;

namespace BOTrasedV3.Interfaces
{
    public interface IFunCommandDataService
    {
        Task<FakeSecret> GetRandomSecretAsync();
    }
}
