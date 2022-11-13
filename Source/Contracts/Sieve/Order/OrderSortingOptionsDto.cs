namespace Contracts.Sieve.Order
{
    public struct OrderSortingOptionsDto
    {
        public OrderSortStateDto IdSort { get; private set; }

        public OrderSortStateDto NumberSort { get; private set; }

        public OrderSortStateDto DateSort { get; private set; }

        public OrderSortStateDto ProviderNameSort { get; private set; }

        public OrderSortStateDto Current { get; private set; }

        public OrderSortingOptionsDto(OrderSortStateDto sortOrder)
        {
            IdSort = sortOrder == OrderSortStateDto.IdAsc ? OrderSortStateDto.IdDesc : OrderSortStateDto.IdAsc;
            NumberSort = sortOrder == OrderSortStateDto.NumberAsc ? OrderSortStateDto.NumberDesc : OrderSortStateDto.NumberAsc;
            DateSort = sortOrder == OrderSortStateDto.DateAsc ? OrderSortStateDto.DateDesc : OrderSortStateDto.DateAsc;
            ProviderNameSort = sortOrder == OrderSortStateDto.ProviderNameAsc ? OrderSortStateDto.ProviderNameDesc : OrderSortStateDto.ProviderNameAsc;
            Current = sortOrder;
        }
    }
}