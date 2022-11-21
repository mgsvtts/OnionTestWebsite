namespace Services.Abstractions;

public interface IServiceManager
{
    public IOrderService OrderService { get; }

    public IProviderService ProviderService { get; }

    public IOrderItemService OrderItemService { get; }
}