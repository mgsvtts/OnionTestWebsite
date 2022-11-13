namespace Contracts.Order
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime Date { get; set; }

        public ProviderDto Provider { get; set; }
    }
}