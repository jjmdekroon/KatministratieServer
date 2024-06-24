using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Superkatten.Katministratie.Application.CageCard.Details
{
    public class CageCardDefaultFooterComposer : IComponent
    {
        private IReadOnlyCollection<Superkat> _superkatten { get; init; }

        public CageCardDefaultFooterComposer(IReadOnlyCollection<Superkat> superkatten)
        {
            _superkatten = superkatten;
        }

        public void Compose(IContainer container)
        {
            var foods = _superkatten
                .Select(s => s.FoodType)
                .Distinct()
                .ToList();

            var litterTypes = _superkatten
                .Select(s => s.LitterType)
                .Distinct()
                .ToList();

            var wedFoods = _superkatten
                .Select(s => s.WetFoodAllowed)
                .Distinct()
                .ToList();

            container
                .PaddingTop(10)
                .Column(column =>
                {
                    column.Item()
                        .Text($"Voer toegestaan: {string.Join("-", foods)}");

                    column.Spacing(5);

                    column.Item()
                        .Text($"Kattenbak: {string.Join("-", litterTypes)}");

                    column.Spacing(5);

                    column.Item()
                        .Text($"Nat voer toegestaan: {string.Join("-", wedFoods)}");

                    column.Spacing(5);

                    column.Item()
                        .BorderBottom(1)
                        .PaddingBottom(5)
                        .AlignCenter()
                        .Text($"Kooikaart superkatten (c) {DateTime.UtcNow.Year}")
                        .SemiBold();
                });
        }

    }
}
