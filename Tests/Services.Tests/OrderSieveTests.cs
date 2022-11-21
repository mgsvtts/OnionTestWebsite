using Contracts;
using Contracts.Order;
using Contracts.OrderItem;
using Contracts.Sieve.Order;
using Moq;

namespace Services.Tests;

public class OrderSieveTests
{
    private readonly IDateTimeProvider _time;

    public OrderSieveTests()
    {
        var mock = new Mock<IDateTimeProvider>();
        mock.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2010, 1, 2));
        _time = mock.Object;
    }

    [Fact]
    public void Can_Execute_Filter()
    {
        var selectedProviders = new List<int> { 2 };
        var options = new OrderFilterOptionsDto(CreateProviders(), selectedProviders, _time.GetCurrentDate(),
            new DateTime(2010, 2, 1), "2");
        var filter = new OrderFilter(options);

        var result = filter.Execute(CreateOrders().AsQueryable()).ToList();

        Assert.NotNull(result);
        Assert.All(result, x => x.Number.Contains('2'));
        Assert.True(result.All(x => x.ProviderId == 2));
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void Can_Execute_Sorter()
    {
        var sortState = OrderSortStateDto.NumberDesc;
        var checkOrders = CreateOrders().OrderByDescending(x => x.Number)
            .ThenByDescending(x => x.Id)
            .ToList();
        var sorter = new OrderSorter(sortState);

        var result = sorter.Execute(CreateOrders().AsQueryable()).ToList();

        Assert.NotNull(result);
        for (var i = 0; i < result.Count; i++)
        {
            Assert.Equal(checkOrders[i].Number, result[i].Number);
        }
    }

    private List<OrderDto> CreateOrders()
    {
        var orders = new List<OrderDto>();
        var random = new Random(0);
        for (var i = 0; i < 25; i++)
        {
            var providerId = random.Next(3);
            orders.Add(new OrderDto
            {
                Id = i,
                Number = $"order {i}",
                ProviderId = providerId,
                OrderItems = new List<OrderItemDto> { new() { Name = $"order item {i}", OrderId = i } },
                Date = _time.GetCurrentDate() + TimeSpan.FromDays(i)
            });
        }

        return orders;
    }

    private static List<ProviderDto> CreateProviders()
    {
        var providers = new List<ProviderDto>();

        for (var i = 1; i < 4; i++)
        {
            providers.Add(new ProviderDto { Id = i, Name = $"provider {i}" });
        }

        return providers;
    }

    public interface IDateTimeProvider
    {
        public DateTime GetCurrentDate();
    }
}