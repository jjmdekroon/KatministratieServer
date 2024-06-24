using Superkatten.Katministratie.Domain.Exceptions;
using System;

namespace Superkatten.Katministratie.Domain.Entities;

public class CatchOrigin
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; private set; }
    public CatchOriginType Type { get; private set; }

    public CatchOrigin(string name, CatchOriginType type)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new DomainException($"{nameof(name)} may not me null or empty");
        }

        Name = name;
        Type = type;
    }
}
