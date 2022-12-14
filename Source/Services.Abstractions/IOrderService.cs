using Contracts.Order;
using Contracts.Sieve.Order;

namespace Services.Abstractions;

public interface IOrderService
{
    public Task<OrderDto> GetByIdAsync(int orderId, CancellationToken token = default);

    public Task CreateAsync(OrderForCreationDto orderForCreationDto, CancellationToken token = default);

    public Task DeleteAsync(int orderId, CancellationToken token = default);

    public Task UpdateAsync(OrderForCreationDto orderForCreationDto, CancellationToken token = default);

    public Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken token = default);

    public Task SeedAsync(CancellationToken token = default);

    public Task DeleteSeededAsync(CancellationToken token = default);

    public Task<IQueryable<OrderDto>> SieveAsync(OrderFilterOptionsDto filterOptions, OrderSortStateDto sortOrder,
        CancellationToken token = default);
}