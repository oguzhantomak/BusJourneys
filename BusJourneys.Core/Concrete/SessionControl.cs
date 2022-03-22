using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BusJourneys.Core.Abstract;
using BusJourneys.Core.Helper.Methods;
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

        //Get response content
        var responseString = await RequestToAPI.Request(_configuration, "GetSessionApiUrl", session);

        //Deserialize response content
        var model = JsonSerializer.Deserialize<SessionGetDto>(responseString, new JsonSerializerOptions
        {
            // Case insensitive when deserializing
            PropertyNameCaseInsensitive = true
        });

        //TODO: Check if the session is valid/unvalid
        return model != null ? model : new SessionGetDto();
    }

    public async Task<List<GetBusLocationsResponseDto.DataDto>> GetBusLocations(string key)
    {
        // Create new request model
        var model = new GetBusLocationsRequestDto
        {
            Data = key,
            Date = DateTime.Now.ToString(),
            DeviceSession = new GetBusLocationsRequestDto.DeviceSessionDto
            {
                DeviceId = _session.Data.DeviceId,
                SessionId = _session.Data.SessionId
            },
            Language = "tr-TR"
        };

        //Get response content
        var responseString = await RequestToAPI.Request(_configuration, "GetBusLocationsApiUrl", model);

        //Deserialize response content
        var responseModel = JsonSerializer.Deserialize<GetBusLocationsResponseDto>(responseString, new JsonSerializerOptions
        {
            // Case insensitive when deserializing
            PropertyNameCaseInsensitive = true
        });

        //TODO: Check if the response is valid/unvalid
        return responseModel.Data;
    }

    public async Task<GetBusJourneysResponseDto> GetBusJourneys(int from, int to, DateTime date)
    {
        // Create new request model
        var requestModel = new GetBusJourneysRequestDto()
        {
            // Set the session information
            DeviceSession = new GetBusJourneysRequestDto.DeviceSessionModel
            {
                SessionId = _session.Data.SessionId,
                DeviceId = _session.Data.DeviceId
            },
            // Set the journey information
            Data = new GetBusJourneysRequestDto.DataDto
            {
                OriginId = from,
                DestinationId = to,
                // Changed date format to API documentation
                DepartureDate = date.ToString("yyyy-MM-dd"),
            },

            // Changed date format to API documentation
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Language = "tr-TR"
        };

        //Get response content
        var responseString = await RequestToAPI.Request(_configuration, "GetBusJourneysApiUrl", requestModel);

        //Deserialize response content
        var responseModel = JsonSerializer.Deserialize<GetBusJourneysResponseDto>(responseString, new JsonSerializerOptions
        {
            // Case insensitive when deserializing
            PropertyNameCaseInsensitive = true
        });
        return responseModel;
    }
}