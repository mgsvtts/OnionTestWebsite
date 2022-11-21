namespace Contracts.Sieve.Order;

public readonly struct OrderSortingOptionsDto
{
    public OrderSortStateDto IdSort { get; }

    public OrderSortStateDto NumberSort { get; }

    public OrderSortStateDto DateSort { get; }

    public OrderSortStateDto ProviderNameSort { get; }

    public OrderSortStateDto Current { get; }

    public OrderSortingOptionsDto(OrderSortStateDto sortOrder)
    {
        IdSort = sortOrder == OrderSortStateDto.IdAsc ? OrderSortStateDto.IdDesc : OrderSortStateDto.IdAsc;
        NumberSort = sortOrder == OrderSortStateDto.NumberAsc
            ? OrderSortStateDto.NumberDesc
            : OrderSortStateDto.NumberAsc;
        DateSort = sortOrder == OrderSortStateDto.DateAsc ? OrderSortStateDto.DateDesc : OrderSortStateDto.DateAsc;
        ProviderNameSort = sortOrder == OrderSortStateDto.ProviderNameAsc
            ? OrderSortStateDto.ProviderNameDesc
            : OrderSortStateDto.ProviderNameAsc;
        Current = sortOrder;
    }
}