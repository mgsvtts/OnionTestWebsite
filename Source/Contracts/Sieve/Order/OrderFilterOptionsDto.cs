using Microsoft.AspNetCore.Mvc.Rendering;

namespace Contracts.Sieve.Order
{
    public readonly struct OrderFilterOptionsDto
    {
        public string? Number { get; }

        public int[] ProviderIds { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public SelectList Providers { get; }

        public OrderFilterOptionsDto(IList<ProviderDto> providers, int[] providerIds, DateTime? fromDate, DateTime? toDate, string? number = null)
        {
            Number = number;
            ProviderIds = providerIds;
            providers = providers.OrderBy(x => x.Name)
                                 .Distinct()
                                 .ToList();
            providers.Insert(0, new ProviderDto { Id = 0, Name = "Все" });
            Providers = new SelectList(providers, "Id", "Name", providerIds);
            FromDate = fromDate != null ? fromDate.Value : DateTime.Today.AddMonths(-1);
            ToDate = toDate != null ? toDate.Value : DateTime.Today;
        }
    }
}