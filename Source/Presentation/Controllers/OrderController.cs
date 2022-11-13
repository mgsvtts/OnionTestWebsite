using Contracts.Order;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public OrderController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public IActionResult Create()
        {
            return View(new OrderForCreationDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderForCreationDto orderDto, CancellationToken token)
        {
            await _serviceManager.OrderService.CreateAsync(orderDto, token);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Edit()
        {
            return View(new OrderForCreationDto());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderForUpdateDto orderDto, CancellationToken token)
        {
            await _serviceManager.OrderService.UpdateAsync(orderDto, token);

            return RedirectToAction(nameof(Show), new { id = orderDto.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            await _serviceManager.OrderService.DeleteAsync(id, token);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Show(int id, CancellationToken token)
        {
            return View(await _serviceManager.OrderService.GetByIdAsync(id, token));
        }

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
    }
}