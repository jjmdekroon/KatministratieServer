using Microsoft.AspNetCore.Http;
using Superkatten.Katministratie.Application.Exceptions;
using Superkatten.Katministratie.Application.Helpers;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Domain.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;
using Superkatten.Katministratie.Infrastructure.Interfaces;
using Superkatten.Katministratie.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BcryptNet = BCrypt.Net.BCrypt;

namespace Superkatten.Katministratie.Application.Services;

public class AdoptionService : IAdoptionService
{
    private readonly IMailService _mailService;
    private readonly ISuperkattenRepository _superkattenRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IUserAuthorisationRepository _userAuthorisationRepository;

    public AdoptionService(
        ILocationRepository locationRepository,
        ISuperkattenRepository superkattenRepository,
        IMailService mailService,
        IUserAuthorisationRepository userAuthorisationRepository
    )
    {
        _mailService = mailService;
        _superkattenRepository = superkattenRepository;
        _locationRepository = locationRepository;
        _userAuthorisationRepository = userAuthorisationRepository;
    }

    public async Task StartSuperkattenAdoptionAsync(StartAdoptionSuperkattenParameters parameters)
    {
        var locations = await _locationRepository.GetLocationsAsync();
        var adoptant = locations
            .Where(o => o.LocationNaw.Name == parameters.AdoptantName)
            .Select(o => (Adoptant)o)
            .FirstOrDefault();

        adoptant ??= await CreateAdoptant(parameters.AdoptantName, parameters.AdoptantEmail);

        var generatedPassword = RandomStringGenerator.Generate(10);
        var user = CreateTemporaryUser(parameters.AdoptantName, parameters.AdoptantEmail, generatedPassword);

        var sendResult = await SendStartAdoptionEmailAsync(adoptant, user, generatedPassword, parameters.Superkatten);

        if (sendResult)
        {
            await Task.WhenAll(parameters.Superkatten.Select(StartAdoptionAsync));
        }
    }

    private User CreateTemporaryUser(string adoptantName, string email, string generatedPassword)
    {
        var username = RandomStringGenerator.Generate(5).ToLower();
        var userExsist = _userAuthorisationRepository
            .GetAllUsers()
            .Any(x => x.Username == adoptantName);

        // hash password
        var passwordHash = BcryptNet.HashPassword(generatedPassword);

        // create domain user model
        var user = new User
        {
            Name = adoptantName,
            Email = email,
            Username = username,
            PasswordHash = passwordHash,
            Permissions = new List<PermissionEnum>() { PermissionEnum.Adopter}
        };

        // save user
        _userAuthorisationRepository.StoreUser(user);

        return user;
    }

    private async Task StartAdoptionAsync(Guid superkatId)
    {
        var superkat = await _superkattenRepository.GetSuperkatAsync(superkatId);
        superkat.StartAdoption();
        await _superkattenRepository.UpdateSuperkatAsync(superkat);
    }

    private async Task<Adoptant> CreateAdoptant(string adoptantName, string adoptantEmail)
    {
        var adoptant = new Adoptant(adoptantName, null, null, null, null, adoptantEmail);
        await _locationRepository.CreateLocationAsync(adoptant);
        return adoptant;
    }

    private async Task<bool> SendStartAdoptionEmailAsync(Adoptant adopter, User user, string generatedPassword, IReadOnlyCollection<Guid> superkatten)
    {
        if (adopter.LocationNaw.Email is null)
        {
            return false;
        }

        var bodyText = $"<html><body>Beste {adopter.LocationNaw.Name}{Environment.NewLine}" + 
            $"{Environment.NewLine}" + 
            $"Je hebt gekozen om het adoptie proces in gang te zetten voor de volgende katten:{Environment.NewLine}" + 
            await CreateLineWithCatInfoAsync(superkatten) + 
            $"{Environment.NewLine}" +

            $"Voor het adoptie process zal je bij het ophalen van de katten een aantal formulieren moeten ondertekenen.{Environment.NewLine}" + 
            $"{Environment.NewLine}" + 

            $"Ga naar: [https://katministratie.azurewebsites.net/Adoption/{adopter.Id}/Start] om verder te gaan. {Environment.NewLine}" + 
            $"Login met user {user.Username} en wachtwoord {generatedPassword}{Environment.NewLine}" +
            $"Vul hier alle info in die we nodig hebben om de adoptie in orde te maken.{Environment.NewLine}" + 
            $"{Environment.NewLine}" + 

            $"Met vriendelijke groet,{Environment.NewLine}" + 
            $"Stichting Superkatten{Environment.NewLine}" + 

            $"{Environment.NewLine}" +
            $"{Environment.NewLine}" +
            $"Bijlagen:" + 
            $"- Adoptievoorwaarden: https://katministratie.azurewebsites.net/voorwaarden_adoptie.pdf{Environment.NewLine}" +
            $"- Adoptieformulier: https://katministratie.azurewebsites.net/ander_formulier_adoptie.pdf{Environment.NewLine}" +
            $"{Environment.NewLine}</body></html>";

        _ = await _mailService.MailToAsync(
            email: adopter.LocationNaw.Email,
            subject: "Adoptie stichting superkatten",
            bodyText: bodyText,
            documentData: Array.Empty<byte>()
        );

        return true;
    }

    private async Task<string> CreateLineWithCatInfoAsync(IReadOnlyCollection<Guid> superkatten)
    {
        var line = string.Empty;
        foreach (var superkatId in superkatten)
        {
            var superkat = await _superkattenRepository.GetSuperkatAsync(superkatId);
            if (superkat == null) 
            {
                continue;
            }

            line += $"{superkat.Name} met nummer {superkat.UniqueNumber} {Environment.NewLine}";
        }
        return line;
    }
}
