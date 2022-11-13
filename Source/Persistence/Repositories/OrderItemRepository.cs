using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationContext _context;

        public OrderItemRepository(ApplicationContext context) => _context = context;

        public async Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token = default)
            => await _context.OrderItem.ToListAsync(token);

        public async Task<IEnumerable<OrderItem>> GetAllByNameAsync(string name, CancellationToken token = default)
            => await _context.OrderItem.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync(token);
    }
}
