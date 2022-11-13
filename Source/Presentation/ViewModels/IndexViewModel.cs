using Contracts.Order;
using Contracts.Sieve.Order;

namespace Presentation.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<OrderDto> Orders { get; set; }

        public OrderFilterOptionsDto FilterOptions { get; set; }

        public OrderSortingOptionsDto SortingOptions { get; set; }
    }
}