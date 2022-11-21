using Contracts.Order;
using Contracts.Sieve.Order;
using Services.Abstractions;

namespace Services;

public class OrderSorter : ISorter<OrderDto>
{
    public OrderSorter(OrderSortStateDto sortOrder)
    {
        SortOrder = sortOrder;
    }

    public OrderSortStateDto SortOrder { get; }

    public IQueryable<OrderDto> Execute(IQueryable<OrderDto> toSort)
    {
        return toSort = SortOrder switch
        {
            OrderSortStateDto.IdDesc => toSort.OrderByDescending(x => x.Id),
            OrderSortStateDto.NumberAsc => toSort.OrderBy(x => x.Number).ThenBy(x => x.Id),
            OrderSortStateDto.NumberDesc => toSort.OrderByDescending(x => x.Number).ThenByDescending(x => x.Id),
            OrderSortStateDto.DateAsc => toSort.OrderBy(x => x.Date).ThenBy(x => x.Number),
            OrderSortStateDto.DateDesc => toSort.OrderByDescending(x => x.Date).ThenByDescending(x => x.Number),
            OrderSortStateDto.ProviderNameAsc => toSort.OrderBy(x => x.Provider.Name).ThenBy(x => x.Number),
            OrderSortStateDto.ProviderNameDesc => toSort.OrderByDescending(x => x.Provider.Name)
                .ThenByDescending(x => x.Number),
            _ => toSort.OrderBy(x => x.Id)
        };
    }
}