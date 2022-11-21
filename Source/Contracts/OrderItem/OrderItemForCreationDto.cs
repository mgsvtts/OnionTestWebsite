using System.ComponentModel.DataAnnotations;

namespace Contracts.OrderItem;

public class OrderItemForCreationDto
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    [Required(ErrorMessage = "Select a name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Select a quantity")]
    public string Quantity { get; set; }

    [Required(ErrorMessage = "Select a unit")]
    public string Unit { get; set; }
}