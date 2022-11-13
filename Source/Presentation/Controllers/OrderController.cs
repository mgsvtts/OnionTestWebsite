using Contracts.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Create()
        {
            var providers = await _serviceManager.ProviderService.GetAllAsync();
            providers = providers.OrderBy(x => x.Name).Distinct();

            return View(new OrderForCreationDto
            {
                Providers = new SelectList(providers, "Id", "Name")
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderForCreationDto orderDto, CancellationToken token)
        {
            try
            {
                await _serviceManager.OrderService.CreateAsync(orderDto, token);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(nameof(orderDto.Number), ex.Message);

                var providers = await _serviceManager.ProviderService.GetAllAsync(token);
                orderDto.Providers = new SelectList(providers, "Id", "Name");

                return View(orderDto);
            }
           
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Update() => View(new OrderForUpdateDto());

        [HttpPost]
        public async Task<IActionResult> Update(OrderForUpdateDto orderDto, CancellationToken token)
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
            => View(await _serviceManager.OrderService.GetByIdAsync(id, token));

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