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
        private static readonly string TestOrderSuffix = "TEST_Number";
        private static readonly string TestProviderSuffix = "TEST_Provider";

        public OrderService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task CreateAsync(OrderForCreationDto orderForCreationDto, CancellationToken token = default)
        {
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

        public async Task DeleteSeeded(CancellationToken token = default)
        {
            var orders = await _repositoryManager.OrderRepository.GetAllByNumberAsync(TestOrderSuffix, token);
            var providers = await _repositoryManager.ProviderRepository.GetAllByNameAsync(TestProviderSuffix, token);

            _repositoryManager.OrderRepository.RemoveRange(orders);
            _repositoryManager.ProviderRepository.RemoveRange(providers);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
        }

        private static IEnumerable<Order> GetSeedData()
        {
            var provider1 = new Provider { Name = TestProviderSuffix + " 1" };
            var provider2 = new Provider { Name = TestProviderSuffix + " 2" };

            var random = new Random(0);

            var orders = new List<Order>();

            for (int i = 0; i <= 500; i++)
            {
                var randomNum = random.Next(2);
                orders.Add(new Order
                {
                    Number = TestOrderSuffix + " " + i,
                    Provider = randomNum == 1 ? provider1 : provider2,
                    Date = DateTime.Today - TimeSpan.FromDays(i)
                });
            }

            return orders;
        }
    }
}