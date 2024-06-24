namespace Superkatten.Katministratie.Host.Pages.Users;

public sealed class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool AllFilledIn => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
}
