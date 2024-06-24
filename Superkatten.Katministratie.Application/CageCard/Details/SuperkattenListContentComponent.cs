using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Superkatten.Katministratie.Application.CageCard.Details;

public class SuperkattenListContentComponent : IComponent
{
    private IReadOnlyCollection<Superkat> _superkatten { get; init; }

    public SuperkattenListContentComponent(IReadOnlyCollection<Superkat> superkatten)
    {
        _superkatten = superkatten;
    }
    public void Compose(IContainer container)
    {
        container
            .Padding(1)
            .Column(column =>
            {
                foreach (var superkat in _superkatten)
                {
                    column.Item().Text(superkat.UniqueNumber);
                }
            });
    }
}
