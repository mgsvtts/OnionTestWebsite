using Contracts;

namespace Services.Abstractions
{
    public interface IOrderService
    {
        public Task CreateAsync(OrderForCreationDto orderForCreationDto, CancellationToken token = default);

        public Task DeleteAsync(int orderId, CancellationToken token = default);

        public Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken token = default);

        public Task SeedAsync(CancellationToken token = default);

        public Task DeleteSeeded(CancellationToken token = default);
    }
}