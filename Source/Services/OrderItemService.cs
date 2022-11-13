using Contracts;
using Domain.Repositories;
using Mapster;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IRepositoryManager _repositoryManager;

        public OrderItemService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllAsync(CancellationToken token = default) 
        {
            var items =await _repositoryManager.OrderItemRepository.GetAllAsync(token);

            return items.Adapt<IEnumerable<OrderItemDto>>();
        }
    }
}
