using Contracts.OrderItem;

namespace Services.Abstractions;

public interface IOrderItemService
{
    public Task<OrderItemDto> GetByIdAsync(int id, CancellationToken token = default);

    public Task CreateAsync(OrderItemForCreationDto orderItemDto, CancellationToken token = default);

    public Task DeleteAsync(int id, CancellationToken token = default);

    public Task UpdateAsync(OrderItemForCreationDto itemDro, CancellationToken token = default);

    public Task<IEnumerable<OrderItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<IEnumerable<OrderItemDto>> GetAllByOrderIdAsync(int orderId, CancellationToken token = default);

    public Task<IEnumerable<string>> GetAllUnitsAsync(CancellationToken token = default);

    public Task<IEnumerable<string>> GetAllNamesAsync(CancellationToken token = default);
}