using Microsoft.Extensions.Configuration;
using System;

namespace Superkatten.Katministratie.Application.Helpers;

public class EmailSettings : IEmailSettings
{
    // See: https://docs.microsoft.com/en-us/azure/app-service/reference-app-settings?tabs=kudu%2Cdotnet

    public string ENVIRONMENT_VAR_SMTP_HOST_ADDRESS = "APPSETTING_SmtpAddress";
    public string ENVIRONMENT_VAR_SMTP_PORT_NUMBER = "APPSETTING_SmtpPortNumber";

    private readonly IConfiguration _configuration;

    public EmailSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string SmtpHost => Environment.GetEnvironmentVariable(ENVIRONMENT_VAR_SMTP_HOST_ADDRESS) ?? string.Empty;
    //_configuration.GetValue<string>(ENVIRONMENT_VAR_SMTP_HOST_ADDRESS) ?? string.Empty;

    public int SmtpPortNumber
    {
        get
        {
            var portNumberAsString = Environment.GetEnvironmentVariable(ENVIRONMENT_VAR_SMTP_PORT_NUMBER);
            _ = int.TryParse(portNumberAsString, out var portNumber);

            return portNumber;
        }
    }
    // _configuration.GetValue<int>(ENVIRONMENT_VAR_SMTP_PORT_NUMBER);
}