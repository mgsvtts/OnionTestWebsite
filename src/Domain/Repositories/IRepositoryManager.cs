namespace Domain.Repositories
{
    public interface IRepositoryManager
    {
        public IOrderRepository OrderRepository { get; }

        public IProviderRepository ProviderRepository { get; }

        public IUnitOfWork UnitOfWork { get; }
    }
}