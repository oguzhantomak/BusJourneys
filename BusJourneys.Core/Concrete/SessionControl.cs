using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BusJourneys.Core.Abstract;
using BusJourneys.Core.Models.Requests;
using BusJourneys.Core.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Wangkanai.Detection.Services;

namespace BusJourneys.Core.Concrete;

public class SessionControl : ISessionControl
{
    //In every request, the session will be checked.
    private SessionGetDto _session;

    //To access configuration settings(appsettings.json)
    private readonly IConfiguration _configuration;

    public SessionControl(IConfiguration configuration, IHttpContextAccessor httpContext, IDetectionService detection)
    {
        //USING
        // 1- IHttpContextAccessor for accessing session
        // 2- IDetectionService for detecting browser

        //The other way to get user browser informations from request header
        //Get user browser
        //var userAgent = Request.Headers["User-Agent"];        

        _configuration = configuration;

        //Client session control
        if (httpContext.HttpContext.Session.GetString("client") != null)
        {
            // Deserialize the session if client is not null
            _session = JsonSerializer.Deserialize<SessionGetDto>(httpContext.HttpContext.Session.GetString("client"));
        }
        else

        {
            // Create new client session with ip address and detect browser
            _session = CreateSession(httpContext.HttpContext.Connection.RemoteIpAddress.ToString(), detection).Result;

            //Set session to client

        }
    }

    public async Task<SessionGetDto> CreateSession(string ip, IDetectionService detection)
    {
        // Create new client session with ip address and detect browser
        var session = new SessionCreateDto
        {
            Type = 1,
            Browser = new Browser()
            {
                BrowserVersion = detection.Browser.Version.ToString(),
                BrowserName = detection.Browser.Name.ToString(),
            },
            Connection = new Connection
            {
                Ip = ip == "::1" ? "78.191.36.29" : ip,

                //?? How can I get the port number?
                Port = "80"
            }
        };

        using var client = new HttpClient();

        //Set token to header
        client.DefaultRequestHeaders.Add("Authorization", _configuration.GetSection("Token").Value);

        //Method type and getting request url from appsettings.json
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(_configuration.GetSection("GetSessionApiUrl").Value));

        //Set request content
        request.Content = new StringContent(JsonSerializer.Serialize(session), Encoding.UTF8, "application/json");
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await client.SendAsync(request);

        //Get response content
        var responseString = await response.Content.ReadAsStringAsync();

        //Deserialize response content
        var model = JsonSerializer.Deserialize<SessionGetDto>(responseString, new JsonSerializerOptions
        {
            // Case insensitive when deserializing
            PropertyNameCaseInsensitive = true
        });

        //TODO: Check if the session is valid/unvalid
        return model != null ? model : new SessionGetDto();
    }
}