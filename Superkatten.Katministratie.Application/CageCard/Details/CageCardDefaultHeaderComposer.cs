using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Host.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Superkatten.Katministratie.Application.CageCard.Details;

public class CageCardDefaultHeaderComposer : IComponent
{
    private IReadOnlyCollection<Superkat> _superkatten { get; init; }

    public CageCardDefaultHeaderComposer(IReadOnlyCollection<Superkat> superkatten)
    {
        _superkatten = superkatten;
    }

    public void Compose(IContainer container)
    {
        var titleStyle = TextStyle
            .Default
            .FontSize(18)
            .SemiBold();

        container
            .Border(1)
            .PaddingBottom(5)
            .Row(row =>
            {
                row.RelativeItem()
                    .Text(GetCageNumberHeaderText(_superkatten))
                    .Style(titleStyle)
                    .FontSize(20);

                row.RelativeItem()
                    .Text(GetFirstCatchDate(_superkatten))
                    .Style(titleStyle)
                    .FontSize(20);

                row.RelativeItem()
                    .Text(GetCatchOrigin(_superkatten))
                    .Style(titleStyle)
                    .FontSize(20);
            });
    }

    private static string GetCatchOrigin(IReadOnlyCollection<Superkat> superkatten)
    {
        var firstCatchOrigin = superkatten
            .OrderBy(s => s.CatchDate)
            .ToList()
            .Select(s => s.CatchOrigin)
            .FirstOrDefault();

        return firstCatchOrigin?.Name ?? string.Empty;
    }

    private static string GetFirstCatchDate(IReadOnlyCollection<Superkat> superkatten)
    {
        var firstCatchDate = superkatten
            .OrderBy(s => s.CatchDate)
            .ToList()
            .Select(s => s.CatchDate)
            .FirstOrDefault();

        return firstCatchDate.ToShortDateString();
    }

    private static string GetCageNumberHeaderText(IReadOnlyCollection<Superkat> superkatten)
    {
        var superkat = superkatten.FirstOrDefault();

        return LocationDisplayConverter.ConvertLocation(superkat?.Location);
    }

    private static string ConvertCatAreaToShowString(CatArea catArea)
    {
        return catArea switch
        {
            CatArea.Quarantine => "Q",
            CatArea.Infirmary => "ZB",
            CatArea.SmallEnclosure => string.Empty,
            CatArea.BigEnclosure => string.Empty,
            CatArea.Room2 => string.Empty,
            _ => string.Empty
        };
    }
}
