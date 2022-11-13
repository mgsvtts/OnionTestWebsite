using Contracts.Order;
using Contracts.Sieve.Order;
using Services.Abstractions;

namespace Services
{
    public class OrderFilter : IFilter<OrderDto>
    {
        public OrderFilterOptionsDto FilterOptions { get; }

        public OrderFilter(OrderFilterOptionsDto filterOptions)
        {
            FilterOptions = filterOptions;
        }

        public IQueryable<OrderDto> Execute(IQueryable<OrderDto> toFilter)
        {
            if (!String.IsNullOrEmpty(FilterOptions.Number))
            {
                toFilter = toFilter.Where(x => x.Number.Contains(FilterOptions.Number));
            }

            if (FilterOptions.CurrentProviderIds.Any() && !FilterOptions.CurrentProviderIds.Contains(0))
            {
                toFilter = from item in toFilter
                           from id in FilterOptions.CurrentProviderIds
                           where item.Provider.Id == id
                           select item;
            }

            toFilter = toFilter.Where(x => x.Date.Date >= FilterOptions.FromDate && x.Date.Date <= FilterOptions.ToDate);

            return toFilter;
        }
    }
}