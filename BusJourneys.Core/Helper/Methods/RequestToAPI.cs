using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BusJourneys.Core.Models.Requests;
using BusJourneys.Core.Models.Responses;
using Microsoft.Extensions.Configuration;

namespace BusJourneys.Core.Helper.Methods;

public static class RequestToAPI
{
    public static async Task<string> Request<T>(IConfiguration _configuration, string key, T model)
    {
        using var client = new HttpClient();

        //Set token to header
        client.DefaultRequestHeaders.Add("Authorization", _configuration.GetSection("Token").Value);

        //Method type and getting request url from appsettings.json
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(_configuration.GetSection(key).Value));

        //Set request content
        request.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //Send request
        HttpResponseMessage response = await client.SendAsync(request);

        //Get response content
        return await response.Content.ReadAsStringAsync();
    }
}