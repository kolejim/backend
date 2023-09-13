using System.Text;
using Api.Biz;
using Api.Controllers.Models;
using Api.Crawler.Ted;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    EncryptionService _encryptionService;

    public AuthController(EncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    [HttpPost]
    [Route("login")]
    public CreateTokenResult Login([FromBody] LoginRequest request)
    {
        bool success;
        string token = "";
        // todo SOLID
        TedClient client = new TedClient();
        try
        {
            client.LoadStudent(request.UserName, request.Password);
            success = true;
            token = Encrypt(request.UserName, request.Password);
        }
        catch (Exception e)
        {
            // todo add message by exception 
            Console.WriteLine(e);
            success = false;
        }
        
        var createTokenResult = new CreateTokenResult() { Token = token, Success = success };

        return createTokenResult;
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    // Get username and password and generate encrypted string
    private string Encrypt(string username, string password)
    {
        var encryptCredentials = _encryptionService.EncryptCredentials(username, password);
        // encode using base64
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptCredentials));
    }

    private Credential Decrypt(string encrypted)
    {
        // decode encrypted parameter using base64
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encrypted));
        var decryptCredentials = _encryptionService.DecryptCredentials(decoded);
        return decryptCredentials;
    }
}