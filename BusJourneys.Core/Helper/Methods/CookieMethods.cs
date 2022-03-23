using BusJourneys.Core.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace BusJourneys.Core.Helper.Methods;

public class CookieMethods
{
    public static async Task SetCookies(HttpResponse response, int from, int to, DateTime date)
    {
        //Cookie settings for last search
        CookieOptions option = new CookieOptions();
        option.Expires = DateTime.Now.AddHours(1);
        response.Cookies.Append("from", from.ToString(), option);
        response.Cookies.Append("to", to.ToString(), option);
        response.Cookies.Append("date", date.ToString(), option);
    }

    public static async Task<GetCookiesDto> GetCookies(HttpRequest request)
    {
        if (request.Cookies["from"] != null && request.Cookies["to"] != null && request.Cookies["date"] != null)
        {
            var model = new GetCookiesDto
            {
                From = request.Cookies["from"],
                To = request.Cookies["to"],
                Date = request.Cookies["date"]
            };

            return model;
        }
        else
        {
            return null;
        }
        
    }
}