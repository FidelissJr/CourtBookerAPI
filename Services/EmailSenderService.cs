using System.Net.Mail;
using System.Net;
using CourtBooker.Repositories.Interfaces;

namespace CourtBooker.Services
{
    public class EmailSenderService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "alugaquadrasjvlle@gmail.com";
            var pw = "lzkf xfqa fbwf uvvy";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pw),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var mailMessage = new MailMessage(from: mail, to: email, subject: subject, body: message);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException($"Ocorreu um erro ao enviar o e-mail: {ex.Message}");
            }
        }

    }

}
