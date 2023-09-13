namespace Api.Controllers.Models;

public class CreateTokenResult
{
    public bool Success { get; set; }
    public string Token { get; set; } = "";

    public string? Message { get; set; }
}