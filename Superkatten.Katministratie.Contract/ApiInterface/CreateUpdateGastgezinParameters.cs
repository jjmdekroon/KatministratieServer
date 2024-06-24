namespace Superkatten.Katministratie.Contract.ApiInterface;

public class LocationNawParameters
{
    public string Name { get; init; } = string.Empty;
    public string? Postcode { get; init; } = string.Empty;
    public string? Address { get; init; } = string.Empty;
    public string? City { get; init; } = string.Empty;
    public string? Phone { get; init; } = string.Empty;
    public string? Email { get; init; } = string.Empty;
}
