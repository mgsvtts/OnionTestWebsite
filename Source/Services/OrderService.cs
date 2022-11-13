using Contracts.Order;
using Contracts.Sieve.Order;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Services.Abstractions;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repositoryManager;
        private static readonly string TestOrderPrefix = "TEST_Number";
        private static readonly string TestProviderPrefix = "TEST_Provider";

        public OrderService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<OrderDto> GetByIdAsync(int orderId, CancellationToken token)
        {
            var order = await _repositoryManager.OrderRepository.GetByIdAsync(orderId, token);

            if (order == null)
            {
                throw new OrderNotFoundException(orderId);
            }

            return order.Adapt<OrderDto>();
        }

        public async Task CreateAsync(OrderForCreationDto orderForCreationDto, CancellationToken token = default)
        {
            var isUnique = await NumberIsUniqueAsync(orderForCreationDto.Number, orderForCreationDto.ProviderId, token);

            if (!isUnique)
            {
                throw new NumberAndProviderIdIsNotUniqueException(orderForCreationDto.Number);
            }

            var order = orderForCreationDto.Adapt<Order>();
            order.Provider = await _repositoryManager.ProviderRepository.GetByIdAsync(orderForCreationDto.ProviderId, token);
            order.Date = DateTime.Now;
            
            foreach(var name in orderForCreationDto.OrderItemNames)
            {
                var item = _repositoryManager.OrderItemRepository.GetAllByNameAsync(name);
            }
           

            _repositoryManager.OrderRepository.Add(order);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(int orderId, CancellationToken token = default)
        {
            var order = await _repositoryManager.OrderRepository.GetByIdAsync(orderId, token);

            if (order == null)
            {
                throw new OrderNotFoundException(orderId);
            }

            _repositoryManager.OrderRepository.Remove(order);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(OrderForUpdateDto orderForUpdateDto, CancellationToken token = default)
        {
            var order = await _repositoryManager.OrderRepository.GetByIdAsync(orderForUpdateDto.Id, token);
            if (order == null)
            {
                throw new OrderNotFoundException(orderForUpdateDto.Id);
            }

            order.Number = orderForUpdateDto.Number;
            order.Provider = await _repositoryManager.ProviderRepository.GetByIdAsync(orderForUpdateDto.ProviderId, token);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken token = default)
        {
            var orders = await _repositoryManager.OrderRepository.GetAllAsync(token);

            return orders.Adapt<IEnumerable<OrderDto>>();
        }

        public async Task<IQueryable<OrderDto>> SieveAsync(OrderFilterOptionsDto filterOptions, OrderSortStateDto sortOrder, CancellationToken token = default)
        {
            var orders = await _repositoryManager.OrderRepository.GetAllAsync(token);
            var ordersDto = orders.Adapt<IEnumerable<OrderDto>>().AsQueryable();

            var filter = new OrderFilter(filterOptions);
            var sorter = new OrderSorter(sortOrder);

            ordersDto = filter.Execute(ordersDto);
            ordersDto = sorter.Execute(ordersDto);

            return ordersDto;
        }

        public async Task SeedAsync(CancellationToken token = default)
        {
            await _repositoryManager.OrderRepository.AddRangeAsync(await GetSeedDataAsync(), token);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
        }

        public async Task DeleteSeededAsync(CancellationToken token = default)
        {
            var orders = await _repositoryManager.OrderRepository.GetAllByNumberAsync(TestOrderPrefix, token);
            var providers = await _repositoryManager.ProviderRepository.GetAllByNameAsync(TestProviderPrefix, token);

            _repositoryManager.OrderRepository.RemoveRange(orders);
            _repositoryManager.ProviderRepository.RemoveRange(providers);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
        }

        private async Task<IEnumerable<Order>> GetSeedDataAsync(CancellationToken token = default)
        {
            int ordersToAdd = 500;

            var random = new Random(0);

            var orders = new List<Order>();
            var providers = new List<Provider>();

            for (int i = 0; i < ordersToAdd; i++)
            {
                var randomProviderId = random.Next(10);

                var provider = providers.FirstOrDefault(x => x.Name.Contains($"{TestProviderPrefix} {randomProviderId}"))
                               ?? await _repositoryManager.ProviderRepository.GetByNameAsync($"{TestProviderPrefix} {randomProviderId}", token);

                if (provider == null)
                {
                    provider = new Provider { Name = $"{TestProviderPrefix} {randomProviderId}" };
                    providers.Add(provider);
                }

                orders.Add(new Order
                {
                    Number = $"{TestOrderPrefix} {i}",
                    Provider = provider,
                    Date = DateTime.Today - TimeSpan.FromDays(i)
                });
            }

            return orders;
        }

        private async Task<bool> NumberIsUniqueAsync(string number, int providerId, CancellationToken token = default)
        {
            var provider = await _repositoryManager.ProviderRepository.GetByIdAsync(providerId, token);
            var orders = await _repositoryManager.OrderRepository.GetAllByNumberAsync(number, token);

            foreach(var order in orders)
            {
                if(order?.Provider==provider)
                {
                    return false;
                }
            }
            return true;
        }
    }
}