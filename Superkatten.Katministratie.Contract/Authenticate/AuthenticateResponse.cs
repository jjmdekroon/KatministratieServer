namespace Superkatten.Katministratie.Contract.Authenticate;

public class AuthenticateResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}