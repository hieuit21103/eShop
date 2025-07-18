namespace Identity.API.Services
{

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        public EmailService(ILogger<EmailService> logger)
        {
            Env.Load();
            Env.TraversePath().Load();
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtp = Environment.GetEnvironmentVariable("SMTP_SERVER");
                var port = Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587";
                var username = Environment.GetEnvironmentVariable("SMTP_USERNAME");
                var password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

                var smtpClient = new SmtpClient(smtp, int.Parse(port))
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username),
                    To = { email },
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email: {ex.Message}");
            }
        }

        public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
        {
            string template = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "ConfirmEmailTemplate.html");
            string body = File.ReadAllText(template);


            var subject = "Email Confirmation";

            body = body.Replace("{{UserEmail}}", email)
                        .Replace("{{ConfirmLink}}", confirmationLink);
            
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            string template = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "ResetPasswordTemplate.html");
            string body = File.ReadAllText(template);

            var subject = "Password Reset";

            body = body.Replace("{{UserEmail}}", email)
                        .Replace("{{ResetLink}}", resetLink);

            await SendEmailAsync(email, subject, body);
        }
    }
}