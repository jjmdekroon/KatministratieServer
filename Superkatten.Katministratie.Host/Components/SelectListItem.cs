namespace Superkatten.Katministratie.Host.Components;

public class SelectListItem<TItem>
{
    public int KeyId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TItem Item{ get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string ItemName { get; set; } = string.Empty;    
}
