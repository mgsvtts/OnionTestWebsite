using System.ComponentModel.DataAnnotations;

namespace Contracts
{
    public class OrderForCreationDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order number is required")]

        public string Number { get; set; }

        public int ProviderId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}