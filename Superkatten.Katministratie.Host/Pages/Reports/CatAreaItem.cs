using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Pages.Reports;

public class CatAreaItem
{
    public int KeyId { get; set; }
    public CatArea CatArea { get; set; }
    public string CatAreaName { get; set; } = string.Empty;
}
