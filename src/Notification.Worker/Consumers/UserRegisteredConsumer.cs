using MassTransit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Worker.Models;
using Shared.Events;

namespace Notification.Worker.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public UserRegisteredConsumer(IOptions<SmtpSettings> smtpSettings, ILogger<UserRegisteredConsumer> logger)
    {
        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received UserRegisteredEvent for {Email}", message.Email);

        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(message.Email));
            email.Subject = "Welcome to eShop! Please confirm your email.";

            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ConfirmEmailTemplate.html");
            
            if (!File.Exists(templatePath))
            {
                 templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ConfirmEmailTemplate.html");
            }

            string body = "";
            if (File.Exists(templatePath))
            {
                body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("{{ConfirmLink}}", message.ConfirmationLink);
                body = body.Replace("{{UserEmail}}", message.Email);
            }
            else
            {
                body = $"<h1>Welcome, {message.Username}!</h1><p>Please confirm your email by clicking here: <a href=\"{message.ConfirmationLink}\">Confirm Email</a></p>";
            }

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.Auto);
            
            if (!string.IsNullOrEmpty(_smtpSettings.Username))
            {
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            }

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Welcome email sent to {Email}", message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", message.Email);
            throw; // Throwing allows MassTransit to retry
        }
    }
}
