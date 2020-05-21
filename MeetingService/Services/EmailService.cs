using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace MeetingsService.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("MeetingsService", "dimataranapa12@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp-relay.gmail.com", 25, false);
                await client.AuthenticateAsync("dimataranapa12@gmail.com", "Dima79919040");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
