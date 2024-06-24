using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Superkatten.Katministratie.Application.Reporting;

public class ReportBuilder : IReportBuilder
{
    public string BuildSuperkattenInventory(IReadOnlyCollection<Superkat> superkatten)
    {
        var result = "Vanglokatie type;vanglokatie name;Total catched;Totaal poezen retour;Totaal katten retour;Totaal kittens retour;Totaal niet retour\n";

        var catchOriginTypes = (CatchOriginType[])Enum.GetValues(typeof(CatchOriginType));

        foreach (var catchOriginType in catchOriginTypes)
        {
            var superkattenAtCatchOriginType = superkatten
                .Where(o => o.CatchOrigin.Type == catchOriginType)
                .ToList();

            var catchOriginNames = superkattenAtCatchOriginType
                .DistinctBy(o => o.CatchOrigin.Name)
                .Select(o => o.CatchOrigin.Name)
                .ToList();

            foreach (var catchOriginName in catchOriginNames)
            {
                result += catchOriginType;
                result += ";";

                result += catchOriginName;
                result += ";";

                var superkattenAtCatchOrigin = superkattenAtCatchOriginType
                    .Where(o => o.CatchOrigin.Name == catchOriginName)
                    .ToList();

                // Total cats catched in total
                result += superkattenAtCatchOrigin
                    .Count(o => o.CatchOrigin.Name == catchOriginName)
                    .ToString("00");
                result += ";";

                // total count of molly's returned
                result += superkattenAtCatchOrigin
                    .Where(o => o.AgeCategory == AgeCategory.Adult || o.AgeCategory == AgeCategory.Juvenile)
                    .Where(o => o.Gender == Gender.Molly)
                    .Count(o => o.Retour)
                    .ToString("00");
                result += ";";

                // total count of molly's returned
                result += superkattenAtCatchOrigin
                    .Where(o => o.AgeCategory == AgeCategory.Adult || o.AgeCategory == AgeCategory.Juvenile)
                    .Where(o => o.Gender == Gender.Tomcat)
                    .Count(o => o.Retour)
                    .ToString("00");
                result += ";";

                // total count of kittens returned
                result += superkattenAtCatchOrigin
                    .Where(o => o.AgeCategory == AgeCategory.Kitten)
                    .Count(o => o.Retour)
                    .ToString("00");
                result += ";";

                // Total count of cats socialized and not returned
                result += superkattenAtCatchOrigin
                    .Count(o => !o.Retour)
                    .ToString("00");
                result += "\n";
            }
        }

        return result;
    }
}
