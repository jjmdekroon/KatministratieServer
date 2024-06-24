using Microsoft.Extensions.Configuration;
using System;

namespace Superkatten.Katministratie.Application.Helpers;

public class ClientSecrets : IClientSecrets
{
    public const string ENVIRONMENT_VAR_GMAIL_CLIENT_ID = "APPSETTING_MailClientId";
    public const string ENVIRONMENT_VAR_GMAIL_CLIENT_SECRET = "APPSETTING_MailClientSecret";

    private readonly IConfiguration _configuration;

    public ClientSecrets(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GmailClientId => Environment.GetEnvironmentVariable(ENVIRONMENT_VAR_GMAIL_CLIENT_ID) ?? string.Empty;
    //_configuration.GetValue<string>(ENVIRONMENT_VAR_GMAIL_CLIENT_ID) ?? string.Empty;
    public string GmailClientSecret => Environment.GetEnvironmentVariable(ENVIRONMENT_VAR_GMAIL_CLIENT_SECRET) ?? string.Empty;
    //_configuration.GetValue<string>(ENVIRONMENT_VAR_GMAIL_CLIENT_SECRET) ?? string.Empty;
};
