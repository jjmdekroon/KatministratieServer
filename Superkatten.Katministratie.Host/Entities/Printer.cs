namespace Superkatten.Katministratie.Host.Entities
{
    public class Printer
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public Printer(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
