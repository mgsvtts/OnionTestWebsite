using Contracts.OrderItem;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Order;

public class OrderForCreationDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Order number is required")]
    public string Number { get; set; }

    [Required(ErrorMessage = "Select a provider")]
    public int ProviderId { get; set; }

    [Required(ErrorMessage = "Data is required")]
    public DateTime Date { get; set; }

    public SelectList Providers { get; set; }

    public List<OrderItemForCreationDto> OrderItems { get; set; }
}