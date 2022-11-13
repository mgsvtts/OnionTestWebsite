using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Order
{
    public class OrderForCreationDto
    {
        [Required(ErrorMessage = "Order number is required")]
        public string Number { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public SelectList Providers { get; set; }

        public List<string> OrderItemNames{ get; set; }  
    }
}