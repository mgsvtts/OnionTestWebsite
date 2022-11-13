using Contracts.Sieve.Order;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;
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

        public async Task<IActionResult> Index(int[] providerIds,
                                               DateTime? fromDate,
                                               DateTime? toDate,
                                               CancellationToken token,
                                               string? number = null,
                                               OrderSortStateDto sortOrder = OrderSortStateDto.IdAsc)
        {
            var providersDto = await _serviceManager.ProviderService.GetAllAsync(token);

            var filterOptions = new OrderFilterOptionsDto(providersDto.ToList(), providerIds, fromDate, toDate, number);

            var ordersDto = await _serviceManager.OrderService.SieveAsync(filterOptions, sortOrder, token);

            return View(new IndexViewModel
            {
                Orders = ordersDto,
                FilterOptions = filterOptions,
                SortingOptions = new OrderSortingOptionsDto(sortOrder)
            }); ;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                Message = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error.Message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}