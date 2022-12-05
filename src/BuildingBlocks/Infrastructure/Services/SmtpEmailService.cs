using Contracts.Configurations;
using Contracts.Interfaces;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SmtpEmailService : ISmtpEmailService
    {
        private readonly ILogger _logger;
        private readonly SmtpEmailSetting _settings;
        private readonly SmtpClient _smtpClinet;

        public SmtpEmailService(ILogger logger, SmtpEmailSetting emailSMTPSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = emailSMTPSettings ?? throw new ArgumentNullException(nameof(emailSMTPSettings));
            _smtpClinet = new SmtpClient();
        }

        public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From),
                Subject = request.Subject,
                Body = new BodyBuilder()
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };

            if (request.ToAddresses.Any())
            {
                foreach (var address in request.ToAddresses)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(address));
                }
            }
            else
            {
                var toAddress = request.ToAddress;
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }

            try
            {
                await _smtpClinet.ConnectAsync(_settings.SMTPServer, _settings.Port,
                    _settings.UseSsl, cancellationToken);
                await _smtpClinet.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                await _smtpClinet.SendAsync(emailMessage, cancellationToken);
                await _smtpClinet.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                await _smtpClinet.DisconnectAsync(true, cancellationToken);
                _smtpClinet.Dispose();
            }
        }
    }
}
