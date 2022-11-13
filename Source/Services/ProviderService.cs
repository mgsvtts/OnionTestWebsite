using Contracts;
using Domain.Repositories;
using Mapster;
using Services.Abstractions;

namespace Services
{
    public class ProviderService : IProviderService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ProviderService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<ProviderDto>> GetAllAsync(CancellationToken token = default)
        {
            var providers = await _repositoryManager.ProviderRepository.GetAllAsync(token);

            return providers.Adapt<IEnumerable<ProviderDto>>();
        }
    }
}