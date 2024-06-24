namespace Superkatten.Katministratie.Application.Helpers;

public interface IEmailSettings
{
    string SmtpHost { get; }
    int SmtpPortNumber { get; }
}