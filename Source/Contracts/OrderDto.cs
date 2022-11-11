namespace Contracts
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime Date { get; set; }

        public string ProviderName { get; set; }
    }
}