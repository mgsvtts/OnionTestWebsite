using Contracts.Order;
using Contracts.OrderItem;
using Contracts.Sieve.Order;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Services.Abstractions;
using System.Text.RegularExpressions;

namespace Services;

public class OrderService : IOrderService
{
    private const string TestOrderPrefix = "TEST_Number";
    private const string TestOrderItemPrefix = "Test_OrderItem";
    private const string TestProviderPrefix = "TEST_Provider";
    private readonly IRepositoryManager _repositoryManager;

    public OrderService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<OrderDto> GetByIdAsync(int orderId, CancellationToken token = default)
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
        await ValidateOrderDtoAsync(orderForCreationDto, token);

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

    public async Task UpdateAsync(OrderForCreationDto orderDto, CancellationToken token = default)
    {
        await ValidateOrderDtoAsync(orderDto, token);

        var order = await _repositoryManager.OrderRepository.GetByIdAsync(orderDto.Id, token);

        order.Number = orderDto.Number;
        order.Provider = await _repositoryManager.ProviderRepository.GetByIdAsync(orderDto.ProviderId, token);
        order.Date = orderDto.Date;

        await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken token = default)
    {
        var orders = await _repositoryManager.OrderRepository.GetAllAsync(token);

        return orders.Adapt<IEnumerable<OrderDto>>();
    }

    public async Task<IQueryable<OrderDto>> SieveAsync(OrderFilterOptionsDto filterOptions, OrderSortStateDto sortOrder,
        CancellationToken token = default)
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
        await _repositoryManager.OrderRepository.AddRangeAsync(await CreateSeedDataAsync(token), token);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
    }

    public async Task DeleteSeededAsync(CancellationToken token = default)
    {
        var orders = await _repositoryManager.OrderRepository.GetAllContainsNumberAsync(TestOrderPrefix, token);
        var providers = await _repositoryManager.ProviderRepository.GetAllByNameAsync(TestProviderPrefix, token);

        _repositoryManager.OrderRepository.RemoveRange(orders);
        _repositoryManager.ProviderRepository.RemoveRange(providers);

        await _repositoryManager.UnitOfWork.SaveChangesAsync(token);
    }

    private async Task<IEnumerable<Order>> CreateSeedDataAsync(CancellationToken token = default)
    {
        const int ordersToAdd = 500;

        var random = new Random(0);

        var orders = new List<Order>();
        var providers = new List<Provider>();
        for (var i = 0; i < ordersToAdd; i++)
        {
            var randomProviderId = random.Next(10);
            var randomItemsCount = random.Next(5);

            var provider = providers.FirstOrDefault(x => x.Name.Contains($"{TestProviderPrefix} {randomProviderId}"))
                           ?? await _repositoryManager.ProviderRepository.GetByNameAsync(
                               $"{TestProviderPrefix} {randomProviderId}", token);

            if (provider == null)
            {
                provider = new Provider { Name = $"{TestProviderPrefix} {randomProviderId}" };
                providers.Add(provider);
            }

            var order = new Order
            {
                Number = $"{TestOrderPrefix} {i}",
                Provider = provider,
                Date = DateTime.Today - TimeSpan.FromDays(i),
                OrderItems = new List<OrderItem>()
            };

            for (var j = 0; j < randomItemsCount; j++)
            {
                order.OrderItems.Add(new OrderItem
                {
                    Name = $"{TestOrderItemPrefix} {random.Next()}",
                    Quantity = random.Next(500),
                    Unit = $"TestUnit {j}"
                });
            }

            orders.Add(order);
        }

        return orders;
    }

    private async Task ValidateOrderDtoAsync(OrderForCreationDto orderDto, CancellationToken token = default)
    {
        var isUnique = await NumberIsUniqueAsync(orderDto.Number, orderDto.ProviderId, token);

        if(orderDto.Id== 0)
        {
            if (!isUnique)
            {
                throw new NumberAndProviderIdIsNotUniqueException(orderDto.Number);
            }
        }
        else
        {
            var orders = await _repositoryManager.OrderRepository.GetAllWithExactlySameNumberAsync(orderDto.Number, token);
            if (!isUnique && orders.Count()>1)
            {
                throw new NumberAndProviderIdIsNotUniqueException(orderDto.Number);
            }
        }

        if (orderDto.Id != 0)
        {
            var order = await _repositoryManager.OrderRepository.GetByIdAsync(orderDto.Id, token);
            if (order == null)
            {
                throw new OrderNotFoundException(orderDto.Id);
            }
        }

        if (orderDto.Number == null)
        {
            throw new ArgumentException("Enter a number");
        }

        if (orderDto.Date == DateTime.MinValue ||
            orderDto.Date == DateTime.MaxValue ||
            !DateTime.TryParse(orderDto.Date.ToString(), out DateTime result))
        {
            throw new FormatException("Date is invalid");
        }

        if (orderDto.OrderItems!=null && orderDto.OrderItems.Any())
        {
            foreach (var item in orderDto.OrderItems)
            {
                if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Unit) || string.IsNullOrEmpty(item.Quantity))
                {
                    throw new ArgumentException("Order item parameters cannot be null");
                }

                if (orderDto.Number == item.Name)
                {
                    throw new OrderItemNumberEqualOrderNameException();
                }

                if (!string.IsNullOrEmpty(item.Quantity) && item.Quantity.Contains('.'))
                {
                    throw new FormatException("Use comma instead of dot in quantity");
                }

                if (!Regex.IsMatch(item.Quantity, @"^[0-9]+(\,[0-9]{1,3})?$"))
                {
                    throw new FormatException("Valid number with maximum 3 decimal places");
                }
            }
        }
    }

    private async Task<bool> NumberIsUniqueAsync(string number, int providerId, CancellationToken token = default)
    {
        var provider = await _repositoryManager.ProviderRepository.GetByIdAsync(providerId, token);
        var orders = await _repositoryManager.OrderRepository.GetAllWithExactlySameNumberAsync(number, token);

        return orders.All(order => order.Provider != provider);
    }
}