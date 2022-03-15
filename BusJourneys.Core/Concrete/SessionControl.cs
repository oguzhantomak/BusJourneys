using BusJourneys.Core.Abstract;
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


    }
}