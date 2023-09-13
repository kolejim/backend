using System.Text;
using Api.Biz;
using Api.Crawler;
using Api.Crawler.Ted;
using Api.Services;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
public class CalendarController : Controller
{
    EncryptionService _encryptionService;
    ILogger _logger;

    public CalendarController(EncryptionService encryptionService, ILogger<CalendarController> logger)
    {
        _encryptionService = encryptionService;
        _logger = logger;
    }
    


    /*
     * Load student's schedule.
     * Get student's username and password as query parameter named 'u'
     * Decrypt 'u' and get username and password
     * Create new TedClient
     * Load student using credentials
     * Generate calendar using student object's schedule property
     * Return calendar as file
     */
    [HttpGet]
    [Route("{credential}")]
    public IActionResult GenerateCalendar(string credential)
    {
        // calculate execution time
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // get request ip address
        var ip = HttpContext.Connection.RemoteIpAddress.ToString();
        _logger.LogInformation("Request from " + ip);
        
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(credential));
        var credentials = _encryptionService.DecryptCredentials(decoded);
        var tedClient = new TedClient();
        var student = tedClient.LoadStudent(credentials.Username, credentials.Password);
        var calendar = CalendarService.GenerateCalendar(student.Schedule);
        // serilaize calendar to ics file
        var serializer = new CalendarSerializer();
        var ics = serializer.SerializeToString(calendar);
        
        // stop execution time
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        _logger.LogInformation("Request from " + ip + " took " + elapsedMs + "ms");
        
        // return ics file
        return File(Encoding.UTF8.GetBytes(ics), "text/calendar", "schedule.ics");
    }


    
    [HttpGet]
    [Route("health")]
    public bool HealthCheck()
    {
        return true;
    }
    

}