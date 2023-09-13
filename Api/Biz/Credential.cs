namespace Api.Biz;

public class Credential
{
    public Credential(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; }
    public string Password { get; }
}
    