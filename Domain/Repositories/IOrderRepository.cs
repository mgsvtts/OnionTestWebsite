using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        public Task<Order?> GetByIdAsync(int orderId, CancellationToken token = default);

        public Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);

        public Task<IEnumerable<Order>> GetAllByNumberAsync(string number, CancellationToken token = default);

        public void Add(Order order);

        public Task AddRangeAsync(IEnumerable<Order> orders, CancellationToken token = default);

        public void Update(Order order);

        public void Remove(Order order);

        public void RemoveRange(IEnumerable<Order> orders);
    }
}