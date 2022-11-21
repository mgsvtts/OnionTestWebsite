using Contracts;

namespace Services.Abstractions;

public interface IProviderService
{
    public Task<IEnumerable<ProviderDto>> GetAllAsync(CancellationToken token = default);
}