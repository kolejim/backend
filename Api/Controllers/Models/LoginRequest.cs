namespace Api.Controllers.Models;

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
}