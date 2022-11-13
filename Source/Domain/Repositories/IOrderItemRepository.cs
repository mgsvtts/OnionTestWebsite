using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IOrderItemRepository
    {
        public Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token= default);

        public Task<IEnumerable<OrderItem>> GetAllByNameAsync(string name, CancellationToken token = default); 
    }
}
