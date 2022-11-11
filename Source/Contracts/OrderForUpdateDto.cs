using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    public class OrderForUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter an order number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Choose a provider")]
        public int ProviderId { get; set; }
    }
}