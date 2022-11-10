using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context) => _context = context;

        public async Task<Order?> GetByIdAsync(int orderId, CancellationToken token = default)
            => await _context.Order.SingleOrDefaultAsync(x => x.Id == orderId, token);

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default)
            => await _context.Order.Include(x => x.Provider).ToListAsync(token);

        public async Task<IEnumerable<Order>> GetAllByNumberAsync(string number, CancellationToken token = default)
            => await _context.Order.Where(x => x.Number.ToLower().Contains(number.ToLower())).ToListAsync(token);

        public void Add(Order order) => _context.Order.Add(order);

        public async Task AddRangeAsync(IEnumerable<Order> orders, CancellationToken token = default)
         => await _context.Order.AddRangeAsync(orders, token);

        public void Update(Order order) => _context.Order.Update(order);

        public void Remove(Order order) => _context.Order.Remove(order);

        public void RemoveRange(IEnumerable<Order> orders) => _context.Order.RemoveRange(orders);
    }
}