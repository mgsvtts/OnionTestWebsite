using Contracts.Order;
using Contracts.OrderItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Abstractions;

namespace Presentation.Controllers;

public class OrderController : Controller
{
    private readonly IServiceManager _serviceManager;

    public OrderController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task<IActionResult> Create(CancellationToken token)
    {
        return View(new OrderForCreationDto
        {
            Providers = await CreateProvidersSelectList(token),
            OrderItems = new()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderForCreationDto orderDto, CancellationToken token)
    {
        try
        {
            await _serviceManager.OrderService.CreateAsync(orderDto, token);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            orderDto.Providers = await CreateProvidersSelectList(token);

            return View(orderDto);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Update(int id, CancellationToken token)
    {
        var order = await _serviceManager.OrderService.GetByIdAsync(id, token);
        var orderItems = await _serviceManager.OrderItemService.GetAllByOrderIdAsync(id, token);

        return View(new OrderForCreationDto
        {
            Id = order.Id,
            Date = order.Date,
            Number = order.Number,
            ProviderId = order.Provider.Id,
            OrderItems = ConvertToOrderItemsForCreationList(orderItems),
            Providers = await CreateProvidersSelectList(token)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Update(OrderForCreationDto orderDto, CancellationToken token)
    {
        try
        {
            await _serviceManager.OrderService.UpdateAsync(orderDto, token);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);

            var itemsDto = await _serviceManager.OrderItemService.GetAllByOrderIdAsync(orderDto.Id, token);

            orderDto.Providers = await CreateProvidersSelectList(token);
            orderDto.OrderItems = ConvertToOrderItemsForCreationList(itemsDto);

            return View(orderDto);
        }

        return RedirectToAction(nameof(Show), new { id = orderDto.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        await _serviceManager.OrderService.DeleteAsync(id, token);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Show(int id, CancellationToken token) => View(await _serviceManager.OrderService.GetByIdAsync(id, token));

    [HttpPost]
    public async Task<IActionResult> Seed(CancellationToken token)
    {
        await _serviceManager.OrderService.SeedAsync(token);

        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSeeded(CancellationToken token)
    {
        await _serviceManager.OrderService.DeleteSeededAsync(token);

        return RedirectToAction(nameof(Index), "Home");
    }

    private async Task<SelectList> CreateProvidersSelectList(CancellationToken token = default)
    {
        var providers = await _serviceManager.ProviderService.GetAllAsync(token);
        providers = providers.OrderBy(x => x.Name).Distinct();

        return new SelectList(providers, "Id", "Name");
    }

    private static List<OrderItemForCreationDto> ConvertToOrderItemsForCreationList(IEnumerable<OrderItemDto> itemsDto)
    {
        var itemsForCreationDto = new List<OrderItemForCreationDto>();
        foreach (var item in itemsDto)
        {
            itemsForCreationDto.Add(new OrderItemForCreationDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity.ToString(),
                OrderId = item.OrderId,
                Unit = item.Unit
            });
        }

        return itemsForCreationDto;
    }
}