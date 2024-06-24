
using Superkatten.Katministratie.Application.CageCard;
using Superkatten.Katministratie.Application.Exceptions;
using Superkatten.Katministratie.Application.Helpers;
using Superkatten.Katministratie.Application.Interfaces;
using Superkatten.Katministratie.Application.Mappers;
using Superkatten.Katministratie.Application.Reporting;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

using ContractEntities = Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Application.Services;

public class ReportingService : IReportingService
{
    private readonly IReportingRepository _reportingRepository;
    private readonly IReportBuilder _reportBuilder;
    private readonly IMailService _mailService;
    private readonly ICageCardProducer _cageCardProducer;
    private readonly ISuperkatMapper _superkatMapper;

    public ReportingService(
        ICageCardProducer cageCardProducer,
        IReportingRepository reportingRepository,
        IReportBuilder reportBuilder,
        IMailService mailService,
        ISuperkatMapper superkatMapper
    )
    {
        _reportingRepository = reportingRepository;
        _reportBuilder = reportBuilder;
        _mailService = mailService;
        _cageCardProducer = cageCardProducer;
        _superkatMapper = superkatMapper;
    }

    public async Task EmailCatchOriginReport(string email, DateTime from, DateTime to)
    {
        if (EmailValidator.IsValidEmail(email))
        {
            throw new ApplicationException($"'{email}' is not a valid email address.");
        }

        var superkatten = await _reportingRepository.GetSuperkattenBetweenFromToAsync(from, to);

        var reportCsvData = _reportBuilder.BuildSuperkattenInventory(superkatten);

        await _mailService.MailToAsync(
            email: email, 
            subject: "Superkatten inventarisatie csv-data", 
            bodyText: reportCsvData, 
            documentData: Array.Empty<byte>()
        );
    }

    public async Task EmailCageCard(string email, ContractEntities.CatArea contractCatArea, int? CageNumber)
    {
        var catArea = _superkatMapper.MapContractToDomain(contractCatArea);

        if (EmailValidator.IsValidEmail(email))
        {
            throw new ApplicationException($"'{email}' is not a valid email address.");
        }

        var superkatten = await _reportingRepository.GetSuperkattenAtLocationAsync(catArea, CageNumber);
        if (superkatten.Count == 0)
        {
            throw new ServiceException($"No superkatten at area '{nameof(catArea)}' and cage number '{CageNumber}'");
        }

        var pdfData = _cageCardProducer.CreateCageCard(superkatten);
        
        await _mailService.MailToAsync(
            email: email,
            subject: $"Kooikaart van ruimte: {catArea} en kooinummer: {CageNumber}.",
            bodyText: $"Hallo,\n\nHierbij de gevraagde kooikaart. Print deze uit en hang de kaart aan de juiste kooi {CageNumber} \n\nGroet,\nKatministrator",
            documentData: pdfData ?? Array.Empty<byte>()
        );       
    }

    public async Task EmailNotNeutralizedAdopteesReport(string email)
    {
        if (EmailValidator.IsValidEmail(email))
        {
            throw new ApplicationException($"'{email}' is not a valid email address.");
        }

        var allNotNeutralizedSuperkatten = await _reportingRepository.GetNotNeutralizedSuperkatten();
        var allNotNeutralizedSuperkattenAtHostFamily = allNotNeutralizedSuperkatten
            .Where(o => o.Location.LocationType != Domain.Entities.Locations.LocationType.Refuge)
            .ToList();

        var pdfData = _cageCardProducer.CreateSuperkattenReport(allNotNeutralizedSuperkattenAtHostFamily);

        await _mailService.MailToAsync(
            email: email,
            subject: $"Rapport",
            bodyText: $"Hallo,\n\nHierbij de gevraagde informatie over alle niet geneutraliseerde katten die zijn geadopteerd. \n\nGroet,\nKatministrator",
            documentData: pdfData ?? Array.Empty<byte>()
        );
    }

    public async Task EmailNotNeutralizedRefugeReport(string email)
    {
        if (EmailValidator.IsValidEmail(email))
        {
            throw new ApplicationException($"'{email}' is not a valid email address.");
        }

        var allNotNeutralizedSuperkatten = await _reportingRepository.GetNotNeutralizedSuperkatten();
        var allNotNeutralizedSuperkattenAtRefuge = allNotNeutralizedSuperkatten
            .Where(o => o.Location.LocationType == Domain.Entities.Locations.LocationType.Refuge)
            .ToList();

        var pdfData = _cageCardProducer.CreateSuperkattenReport(allNotNeutralizedSuperkattenAtRefuge);

        await _mailService.MailToAsync(
           email: email,
           subject: $"Rapport",
           bodyText: $"Hallo,\n\nHierbij de gevraagde informatie over alle niet geneutraliseerde katten in de opvang. \n\nGroet,\nKatministrator",
           documentData: pdfData ?? Array.Empty<byte>()
       );
    }
}
