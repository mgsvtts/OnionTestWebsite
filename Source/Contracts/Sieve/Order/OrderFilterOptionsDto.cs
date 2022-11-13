using Microsoft.AspNetCore.Mvc.Rendering;

namespace Contracts.Sieve.Order
{
    public readonly struct OrderFilterOptionsDto
    {
        public string? Number { get; }

        public IList<int> CurrentProviderIds { get; }

        public MultiSelectList Providers { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public OrderFilterOptionsDto(IList<ProviderDto> providers, IList<int> providerIds, DateTime? fromDate, DateTime? toDate, string? number = null)
        {
            Number = number;
            CurrentProviderIds = providerIds;
            providers = providers.OrderBy(x => x.Name)
                                 .Distinct()
                                 .ToList();
            providers.Insert(0, new ProviderDto { Id = 0, Name = "Все" });
            Providers = new MultiSelectList(providers, "Id", "Name", providerIds);
            FromDate = fromDate != null ? fromDate.Value : DateTime.Today.AddMonths(-1);
            ToDate = toDate != null ? toDate.Value : DateTime.Today;
        }
    }
}