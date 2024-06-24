namespace Superkatten.Katministratie.Contract.ApiInterface.Reporting;

public class RequestCatchOriginEmailParameters
{
    public string Email { get; init; } = string.Empty;
    public DateTime From { get; init; }
    public DateTime To { get; init; }
}
