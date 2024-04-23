using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Sport_Accessories.SettingModels;
using System.Net;
using System.Net.Mail;

namespace Sport_Accessories.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<SMTPSettings> _smtpSettings;
        public EmailSender(IOptions<SMTPSettings> smptSettings)
        {
            this._smtpSettings = smptSettings;
        }

        public async Task SendEmailAsync(string email, string subject,
                                         string htmlMessage)
        {
            using (var smtp = new SmtpClient(_smtpSettings.Value.Host,
                                            _smtpSettings.Value.Port))
            {
                smtp.Credentials = new NetworkCredential(_smtpSettings.Value.User,
                                                         _smtpSettings.Value.Password);
                await smtp.SendMailAsync(_smtpSettings.Value.User, email,
                                         subject, htmlMessage);
            }
        }
    }
}
