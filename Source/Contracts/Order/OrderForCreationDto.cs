using System.ComponentModel.DataAnnotations;

namespace Contracts.Order
{
    public class OrderForCreationDto
    {
        [Required(ErrorMessage = "Order number is required")]
        public string Number { get; set; }

        [Required]
        public int ProviderId { get; set; }
    }
}