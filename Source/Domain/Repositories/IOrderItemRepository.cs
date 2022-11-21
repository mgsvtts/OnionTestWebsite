using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderItemRepository
{
    public Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token = default);

    public Task<OrderItem?> GetByIdAsync(int id, CancellationToken token = default);

    public Task<IEnumerable<OrderItem>> GetAllByNameAsync(string name, CancellationToken token = default);

    public Task<IEnumerable<OrderItem>> GetAllByOrderIdAsync(int orderId, CancellationToken token = default);

    public void Add(OrderItem item);

    public void Remove(OrderItem item);

    public void RemoveRange(IEnumerable<OrderItem> items);
}