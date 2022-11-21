using Domain.Repositories;

namespace Persistence.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IOrderItemRepository> _lazyOrderItemRepository;
    private readonly Lazy<IOrderRepository> _lazyOrderRepository;
    private readonly Lazy<IProviderRepository> _lazyProviderRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(ApplicationContext context)
    {
        _lazyOrderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(context));
        _lazyProviderRepository = new Lazy<IProviderRepository>(() => new ProviderRepository(context));
        _lazyOrderItemRepository = new Lazy<IOrderItemRepository>(() => new OrderItemRepository(context));
        _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
    }

    public IOrderRepository OrderRepository => _lazyOrderRepository.Value;

    public IProviderRepository ProviderRepository => _lazyProviderRepository.Value;

    public IOrderItemRepository OrderItemRepository => _lazyOrderItemRepository.Value;

    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}