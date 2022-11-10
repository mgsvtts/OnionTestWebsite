using Domain.Repositories;
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
    }
}