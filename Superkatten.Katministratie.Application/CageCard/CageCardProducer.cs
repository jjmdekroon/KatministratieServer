using QuestPDF.Fluent;
using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Superkatten.Katministratie.Application.CageCard;

public class CageCardProducer : ICageCardProducer
{
    private const string EXTENTION_PDF = "pdf";

    private readonly ICageCardComposerFactory _composerFactory;

    public CageCardProducer(ICageCardComposerFactory composerFactory)
    {
        _composerFactory = composerFactory;
    }

    public byte[]? CreateCageCard(IReadOnlyCollection<Superkat> superkatten)
    {
        var document = new CageCardDocument(_composerFactory, superkatten);
        var data = document.GeneratePdf();

        return data;
    }

    public byte[]? CreateSuperkattenReport(IReadOnlyCollection<Superkat> superkatten)
    {
        var document = new SuperkattenListDocument(_composerFactory, superkatten);
        var data = document.GeneratePdf();

        return data;
    }
}