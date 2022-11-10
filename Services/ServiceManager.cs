using Domain.Repositories;
using Services.Abstractions;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IOrderService> _lazyOrderService;
        private readonly Lazy<IProviderService> _lazyProviderService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyOrderService = new Lazy<IOrderService>(() => new OrderService(repositoryManager));
            _lazyProviderService = new Lazy<IProviderService>(() => new ProviderService(repositoryManager));
        }

        public IOrderService OrderService => _lazyOrderService.Value;

        public IProviderService ProviderService => _lazyProviderService.Value;
    }
}