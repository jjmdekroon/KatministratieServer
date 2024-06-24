using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;

namespace Superkatten.Katministratie.Application.CageCard.Details;

public class CageCardDefaultContentComposer : IComponent
{
    private const int MAX_COLUMS = 4;
    private IReadOnlyCollection<Superkat> _superkatten { get; init; }

    public CageCardDefaultContentComposer(IReadOnlyCollection<Superkat> superkatten)
    {
        _superkatten = superkatten;
    }

    public void Compose(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Element(ComposeTable);
        });
    }

    private void ComposeTable(IContainer container)
    {
        var columns = _superkatten.Count < MAX_COLUMS
            ? _superkatten.Count
            : MAX_COLUMS;

        container
            .PaddingBottom(5)
            .Grid(grid =>
            {
                grid.Spacing(5);
                grid.AlignCenter();
                grid.Columns(columns);

                foreach (var superkat in _superkatten)
                {
                    var superkatElement = new CageCardSuperkatDetailsComponent(superkat);
                    grid.Item().Element(superkatElement.Compose);
                }

                var reminder = (MAX_COLUMS - _superkatten.Count) % MAX_COLUMS;
                var fillingCount = _superkatten.Count <= MAX_COLUMS
                    ? 0
                    : reminder;

                for (var fillingIndex = 0; fillingIndex < fillingCount; fillingIndex++)
                {
                    grid.Item().Border(1);
                }
            });
    }
}