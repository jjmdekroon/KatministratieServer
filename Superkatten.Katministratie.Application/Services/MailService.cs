using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Superkatten.Katministratie.Application.Helpers;
using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Services;

public class MailService : IMailService
{
    private const string SENDER_NAME = "Katministrator";
    private const string SENDER_EMAIL_ADDRESS = "katministratie@superkatten.nl";

    private readonly IEmailSettings _emailSettings;
    private readonly IClientSecrets _clientSecretSettings;

    public MailService(
        IEmailSettings emailSettings,
        IClientSecrets clientSecretSettings
    )
    {
        _emailSettings = emailSettings;
        _clientSecretSettings = clientSecretSettings;
    }

    public async Task<bool> MailToAsync(string email, string subject, string bodyText, byte[] documentData)
    {
        var message = BuildEmail(email, subject, bodyText, documentData);

        using var client = new SmtpClient();
        var sendResult = false;
        try
        {
            client.Connect(
                _emailSettings.SmtpHost,
                _emailSettings.SmtpPortNumber,
                SecureSocketOptions.Auto
            );

            client.Authenticate(
                _clientSecretSettings.GmailClientId, 
                _clientSecretSettings.GmailClientSecret
            );

            _ = await client.SendAsync(message);

            sendResult = true;
        }
        catch (Exception)
        {
            // no finalize, maybe logging ?
        }

        client.Disconnect(true);

        return sendResult;
    }

    private static MimeMessage BuildEmail(string email, string subject, string bodyText, byte[] documentData)
    {
        // See also https://blog.christian-schou.dk/send-emails-with-asp-net-core-with-mailkit/

        var message = new MimeMessage();
        message.To.Add(new MailboxAddress("Requester", email));
        message.From.Add(new MailboxAddress(SENDER_NAME, SENDER_EMAIL_ADDRESS));
        message.Subject = subject;

        var body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = bodyText
        };

        var stream = new MemoryStream(documentData);

        // create an image attachment for the file located at path
        // see: http://ibgwww.colorado.edu/~lessem/psyc5112/usail/mail/mime/typetxt.html
        var attachment = new MimePart(MediaTypeNames.Application.Pdf)
        {
            Content = new MimeContent(stream),
            ContentId = "test",
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = "test.pdf"
        };

        message.Body = new Multipart("mixed")
        {
            body,
            attachment
        };

        return message;
    }
}