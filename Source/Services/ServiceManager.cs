using Domain.Repositories;
using Services.Abstractions;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IOrderService> _lazyOrderService;
        private readonly Lazy<IProviderService> _lazyProviderService;
        private readonly Lazy<IOrderItemService> _lazyOrderItemService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyOrderService = new Lazy<IOrderService>(() => new OrderService(repositoryManager));
            _lazyProviderService = new Lazy<IProviderService>(() => new ProviderService(repositoryManager));
            _lazyOrderItemService = new Lazy<IOrderItemService>(() => new OrderItemService(repositoryManager));
        }

        public IOrderService OrderService => _lazyOrderService.Value;

        public IProviderService ProviderService => _lazyProviderService.Value;

        public IOrderItemService OrderItemService => _lazyOrderItemService.Value;
    }
}