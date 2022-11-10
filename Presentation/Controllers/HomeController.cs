using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Services.Abstractions;
using System.Diagnostics;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public HomeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index(CancellationToken token)
        {
            return View(await _serviceManager.OrderService.GetAllAsync(token));
        }

        [HttpPost]
        public async Task<IActionResult> SeedOrders(CancellationToken token)
        {
            await _serviceManager.OrderService.SeedAsync(token);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSeededOrders(CancellationToken token)
        {
            await _serviceManager.OrderService.DeleteSeeded(token);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}