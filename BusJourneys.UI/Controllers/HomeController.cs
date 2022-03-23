using BusJourneys.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusJourneys.Core.Abstract;
using BusJourneys.Core.Helper.Methods;
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

        public async Task<IActionResult> Index()
        {
            var busLocations = await _sessionControl.GetBusLocations("");

            var list = new List<GetBusLocationsResponseDto.DataDto>();

            //Try to get ids from cookie
            var cookies = await CookieMethods.GetCookies(Request);

            // When Index loads if cookie is not null use these to set values to "From", "To" and "Date"
            if (cookies != null)
            {
                int fromId = Convert.ToInt32(cookies.From);
                int toId = Convert.ToInt32(cookies.To);

                list.Add(busLocations.Where(x => x.Id == fromId).FirstOrDefault());
                list.Add(busLocations.Where(x => x.Id == toId).FirstOrDefault());
                ViewBag.Date = cookies.Date;
            }
            else
            {
                list.Add(busLocations.First()); //İstanbul Avrupa
                list.Add(busLocations.Skip(2).Take(1).First()); //Ankara for looks like obilet.com

                //Datetime things...........
                DateTime utc = DateTime.UtcNow;
                TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                DateTime time = TimeZoneInfo.ConvertTimeFromUtc(utc, tz);

                ViewBag.Date = time.AddDays(1);

            }

            return View(list);
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

            // Get the bus journeys from api
            var busJourneys = await _sessionControl.GetBusJourneys(from, to, date);

            //Set cookie settings
            await CookieMethods.SetCookies(Response, from, to, date);

            return View(busJourneys);
        }

        public async Task<IActionResult> GetBusJourneys()
        {
            return RedirectToAction("Index", "Home");
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