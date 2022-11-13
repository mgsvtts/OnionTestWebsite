using Contracts.Order;
using Contracts.Sieve.Order;
using Services.Abstractions;

namespace Services
{
    public class OrderSorter : ISorter<OrderDto>
    {
        public OrderSortStateDto SortOrder { get; }

        public OrderSorter(OrderSortStateDto sortOrder)
        {
            SortOrder = sortOrder;
        }

        public IQueryable<OrderDto> Execute(IQueryable<OrderDto> toSort)
        {
            return toSort = SortOrder switch
            {
                OrderSortStateDto.IdDesc => toSort.OrderByDescending(s => s.Id),
                OrderSortStateDto.NumberAsc => toSort.OrderBy(s => s.Number),
                OrderSortStateDto.NumberDesc => toSort.OrderByDescending(s => s.Number),
                OrderSortStateDto.DateAsc => toSort.OrderBy(s => s.Date),
                OrderSortStateDto.DateDesc => toSort.OrderByDescending(s => s.Date),
                OrderSortStateDto.ProviderNameAsc => toSort.OrderBy(s => s.Provider.Name),
                OrderSortStateDto.ProviderNameDesc => toSort.OrderByDescending(s => s.Provider.Name),
                _ => toSort.OrderBy(s => s.Id),
            };
        }
    }
}