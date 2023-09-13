using System.Text;
using Api.Biz;
using Api.Controllers.Models;
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
    public CreateTokenResult CreateToken([FromBody] LoginRequest request)
    {
        var token = Encrypt(request.UserName, request.Password);

        var createTokenResult = new CreateTokenResult() { Token = token, Success = true };

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