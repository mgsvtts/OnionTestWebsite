using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationContext _context;

    public OrderItemRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token = default)
        => await _context.OrderItem.ToListAsync(token);

    public async Task<OrderItem?> GetByIdAsync(int id, CancellationToken token)
        => await _context.OrderItem.SingleOrDefaultAsync(x => x.Id == id, token);

    public async Task<IEnumerable<OrderItem>> GetAllByNameAsync(string name, CancellationToken token = default)
        => await _context.OrderItem.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync(token);

    public async Task<IEnumerable<OrderItem>> GetAllByOrderIdAsync(int orderId, CancellationToken token = default)
        => await _context.OrderItem.Where(x => x.OrderId == orderId).ToListAsync(token);

    public void Remove(OrderItem item)
        => _context.OrderItem.Remove(item);

    public void RemoveRange(IEnumerable<OrderItem> items)
        => _context.OrderItem.RemoveRange(items);

    public void Add(OrderItem item)
        => _context.OrderItem.Add(item);
}