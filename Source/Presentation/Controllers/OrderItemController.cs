using Contracts.Order;
using Contracts.OrderItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Abstractions;

namespace Presentation.Controllers;

public class OrderItemController : Controller
{
    private readonly IServiceManager _serviceManager;

    public OrderItemController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public IActionResult Create(int orderId) => View(new OrderItemForCreationDto { OrderId = orderId });

    [HttpPost]
    public async Task<IActionResult> Create(OrderItemForCreationDto item, CancellationToken token)
    {
        try
        {
            await _serviceManager.OrderItemService.CreateAsync(item, token);
        }
        catch (Exception ex)
        {
            if (ex.GetType() != typeof(DbUpdateException))
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(item);
        }

        return RedirectToAction(nameof(OrderController.Update), "Order", new { id = item.OrderId });
    }

    public async Task<IActionResult> Update(int id)
    {
        var item =await  _serviceManager.OrderItemService.GetByIdAsync(id);
        return View(new OrderItemForCreationDto
        {
            Id= id,
            OrderId = item.OrderId,
            Name = item.Name,
            Quantity = item.Quantity.ToString(),
            Unit = item.Unit
        });
    }

    [HttpPost]
    public async Task<IActionResult> Update(OrderItemForCreationDto itemDto, CancellationToken token)
    {
        try
        {
            await _serviceManager.OrderItemService.UpdateAsync(itemDto, token);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            return View(itemDto);
        }

        return RedirectToAction(nameof(OrderController.Show),"Order", new { id = itemDto.OrderId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, string? returnUrl, CancellationToken token)
    {
        await _serviceManager.OrderItemService.DeleteAsync(id, token);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Show(int id) => View(await _serviceManager.OrderItemService.GetByIdAsync(id));
}