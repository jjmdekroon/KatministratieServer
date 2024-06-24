namespace Superkatten.Katministratie.Host.Entities;

public class SelectionListModel<T> 
{
    public string Key { get; set; } = string.Empty;
    public T? Value { get; set; }
}
