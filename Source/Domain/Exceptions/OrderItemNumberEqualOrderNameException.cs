namespace Domain.Exceptions;

public sealed class OrderItemNumberEqualOrderNameException : BadRequestException
{
    public OrderItemNumberEqualOrderNameException()
        : base("Order item name cannot be equal to order number")
    {
    }
}