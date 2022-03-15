using BusJourneys.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusJourneys.Core.Abstract;
using BusJourneys.Core.Models.Responses;

namespace BusJourneys.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionControl _sessionControl;


        public HomeController(ILogger<HomeController> logger, ISessionControl sessionControl)
        {
            _logger = logger;
            _sessionControl = sessionControl;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<SearchItemsDto> GetBusLocations(string key)
        {
            // Get the bus locations from service
            var busLocations = await _sessionControl.GetBusLocations(key);

            // Set the bus locations to select2.js template model. Source: https://select2.org/data-sources/ajax
            var model = new SearchItemsDto
            {
                items = busLocations,
                total_count = busLocations.Count
            };
            return model;
        }

        [HttpPost]
        public async Task<IActionResult> GetBusJourneys(int from, int to, DateTime date)
        {
            if (from == 0 || to == 0 || date == null)
            {
                return BadRequest();
            }

            var busJourneys = await _sessionControl.GetBusJourneys(from, to, date);
            return View(busJourneys);
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