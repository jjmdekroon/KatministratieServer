using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services;

public interface IMailService
{
    Task<bool> MailToAsync(string email, string subject, string bodyText, byte[] documentData);


    //TODO: moet zoiets worden zie: https://blog.christian-schou.dk/send-emails-with-asp-net-core-with-mailkit/
    //Task SendAsync(MailData mailData, CancellationToken ct);
    //Task SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken ct);

}

