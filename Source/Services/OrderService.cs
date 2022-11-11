using Contracts;
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
            var result = await _repositoryManager.ProviderRepository.NumberIsUniqueAsync(orderForCreationDto.Number, orderForCreationDto.ProviderId, token);

            if (!result)
            {
                throw new NumberAndProviderIdIsNotUniqueException(orderForCreationDto.Number);
            }

            var order = orderForCreationDto.Adapt<Order>();

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

        public async Task SeedAsync(CancellationToken token = default)
        {
            await _repositoryManager.OrderRepository.AddRangeAsync(GetSeedData(), token);

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

        private static IEnumerable<Order> GetSeedData()
        {
            int ordersToAdd = 500;
            var provider1 = new Provider { Name = TestProviderPrefix + " 1" };
            var provider2 = new Provider { Name = TestProviderPrefix + " 2" };

            var random = new Random(0);

            var orders = new List<Order>();

            for (int i = 0; i <= ordersToAdd; i++)
            {
                var randomNum = random.Next(2);
                orders.Add(new Order
                {
                    Number = TestOrderPrefix + " " + i,
                    Provider = randomNum == 1 ? provider1 : provider2,
                    Date = DateTime.Today - TimeSpan.FromDays(i)
                });
            }

            return orders;
        }
    }
}