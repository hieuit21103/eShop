using MassTransit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Worker.Models;
using Shared.Events;

namespace Notification.Worker.Consumers;

public class UserPasswordResetRequestedConsumer : IConsumer<UserPasswordResetRequestedEvent>
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<UserPasswordResetRequestedConsumer> _logger;

    public UserPasswordResetRequestedConsumer(IOptions<SmtpSettings> smtpSettings, ILogger<UserPasswordResetRequestedConsumer> logger)
    {
        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserPasswordResetRequestedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received UserPasswordResetRequestedEvent for {Email}", message.Email);

        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(message.Email));
            email.Subject = "Reset Password";

            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ResetPasswordTemplate.html");
            
            if (!File.Exists(templatePath))
            {
                 templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ResetPasswordTemplate.html");
            }

            string body = "";
            if (File.Exists(templatePath))
            {
                body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("{{link}}", message.ResetLink);
            }
            else
            {
                body = $"<h1>Reset Password</h1><p>Please reset your password by clicking here: <a href=\"{message.ResetLink}\">Reset Password</a></p>";
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

            _logger.LogInformation("Password reset email sent to {Email}", message.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset email to {Email}", message.Email);
            throw; // Throwing allows MassTransit to retry
        }
    }
}
