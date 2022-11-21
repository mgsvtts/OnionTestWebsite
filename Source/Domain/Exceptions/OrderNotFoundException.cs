namespace Domain.Exceptions;

public sealed class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(int orderId)
        : base($"Order with id: {orderId} was not found")
    {
    }
}