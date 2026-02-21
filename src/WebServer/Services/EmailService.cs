using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading.Tasks;
using Yggdrasil.Logging;

namespace Melia.Web.Services
{
	/// <summary>
	/// Service for sending emails via SMTP.
	/// </summary>
	public class EmailService
	{
		private readonly EmailSettings _settings;

		/// <summary>
		/// Creates a new instance of the EmailService.
		/// </summary>
		/// <param name="settings">The email configuration settings.</param>
		public EmailService(EmailSettings settings)
		{
			_settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		/// <summary>
		/// Sends a password reset email with the given token.
		/// </summary>
		/// <param name="toEmail">The recipient's email address.</param>
		/// <param name="resetToken">The password reset token (unhashed).</param>
		/// <returns>True if the email was sent successfully, false otherwise.</returns>
		public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetToken)
		{
			try
			{
				var resetUrl = $"{_settings.BaseUrl}/reset-password?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(toEmail)}";

				var subject = $"[{_settings.SenderName}] Password Reset Request";
				var body = $@"Hello,

You have requested to reset your password. Click the link below to proceed:

{resetUrl}

This link will expire in 1 hour.

If you did not request this password reset, please ignore this email. Your account remains secure.

Regards,
{_settings.SenderName}

---
This is an automated message. Please do not reply to this email.";

				return await SendEmailAsync(toEmail, subject, body);
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to send password reset email to {toEmail}: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Sends a confirmation email after a successful password reset.
		/// </summary>
		/// <param name="toEmail">The recipient's email address.</param>
		/// <returns>True if the email was sent successfully, false otherwise.</returns>
		public async Task<bool> SendPasswordResetConfirmationAsync(string toEmail)
		{
			try
			{
				var subject = $"[{_settings.SenderName}] Password Changed Successfully";
				var body = $@"Hello,

Your password has been successfully changed.

If you did not make this change, please contact support immediately.

Regards,
{_settings.SenderName}

---
This is an automated message. Please do not reply to this email.";

				return await SendEmailAsync(toEmail, subject, body);
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to send password confirmation email to {toEmail}: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Sends an email using the configured SMTP settings.
		/// </summary>
		/// <param name="toEmail">The recipient's email address.</param>
		/// <param name="subject">The email subject.</param>
		/// <param name="body">The email body.</param>
		/// <returns>True if the email was sent successfully, false otherwise.</returns>
		public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
		{
			if (!_settings.Enabled)
			{
				Log.Warning("Email service is disabled. Would have sent email to: {0}, Subject: {1}", toEmail, subject);
				return true;
			}

			Log.Info($"Attempting to send email to {toEmail} via {_settings.SmtpHost}:{_settings.SmtpPort}");

			try
			{
				var (canConnect, connectError) = await TestSmtpConnectionAsync();
				if (!canConnect)
				{
					Log.Error($"Cannot connect to SMTP server: {connectError}");
					return false;
				}

				using var client = new SmtpClient
				{
					Host = _settings.SmtpHost,
					Port = _settings.SmtpPort,
					EnableSsl = _settings.UseSsl,
					Timeout = 30000,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword)
				};

				using var message = new MailMessage
				{
					From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
					Subject = subject,
					Body = body,
					IsBodyHtml = false,
					Priority = MailPriority.Normal
				};
				message.To.Add(new MailAddress(toEmail));

				message.Headers.Add("X-Mailer", "MeliaWebServer");
				message.Headers.Add("X-Priority", "3");

				await client.SendMailAsync(message);
				Log.Info($"Email sent successfully to {toEmail}: {subject}");
				return true;
			}
			catch (SmtpFailedRecipientException ex)
			{
				Log.Error($"Recipient rejected: {toEmail} - Status: {ex.StatusCode} - {ex.Message}");
				return false;
			}
			catch (SmtpException ex)
			{
				LogSmtpError(ex);
				return false;
			}
			catch (Exception ex)
			{
				Log.Error($"Email send failed: {ex.GetType().Name} - {ex.Message}");
				if (ex.InnerException != null)
					Log.Error($"Inner: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");
				return false;
			}
		}

		private void LogSmtpError(SmtpException ex)
		{
			Log.Error($"SMTP error: {ex.StatusCode} - {ex.Message}");

			if (ex.InnerException != null)
			{
				Log.Error($"Inner exception: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");

				if (ex.InnerException is System.Security.Authentication.AuthenticationException)
				{
					Log.Error("SSL/TLS authentication failed. This often means:");
					Log.Error("  1. For Gmail: You MUST use an App Password, not your regular password");
					Log.Error("  2. Generate App Password at: https://myaccount.google.com/apppasswords");
					Log.Error("  3. First enable 2-Factor Authentication on your Google account");
				}
				else if (ex.InnerException is System.IO.IOException)
				{
					Log.Error("IO Error - connection was reset. Check firewall and antivirus settings.");
				}
			}

			switch (ex.StatusCode)
			{
				case SmtpStatusCode.MustIssueStartTlsFirst:
					Log.Error("Server requires TLS. Ensure UseSsl is set to true.");
					break;
				case SmtpStatusCode.ClientNotPermitted:
					Log.Error("Authentication failed. For Gmail, use an App Password!");
					break;
				case SmtpStatusCode.MailboxUnavailable:
					Log.Error("Mailbox unavailable. The recipient address may be invalid.");
					break;
				case SmtpStatusCode.GeneralFailure:
					Log.Error("General failure. Common causes:");
					Log.Error("  - Wrong password (for Gmail: use App Password, not regular password)");
					Log.Error("  - Firewall blocking outgoing connections on port 587");
					Log.Error("  - Antivirus/security software blocking SMTP");
					Log.Error("  - ISP blocking outgoing SMTP");
					break;
			}
		}

		/// <summary>
		/// Tests if we can establish a TCP connection to the SMTP server.
		/// </summary>
		private async Task<(bool success, string error)> TestSmtpConnectionAsync()
		{
			try
			{
				using var client = new TcpClient();
				var connectTask = client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort);

				if (await Task.WhenAny(connectTask, Task.Delay(5000)) != connectTask)
					return (false, $"Connection to {_settings.SmtpHost}:{_settings.SmtpPort} timed out");

				await connectTask;
				return (true, null);
			}
			catch (SocketException ex)
			{
				var error = $"Socket error: {ex.SocketErrorCode} - {ex.Message}";
				Log.Error(error);
				Log.Error("Check: 1) Firewall settings, 2) SMTP host/port, 3) Internet connection");
				return (false, error);
			}
			catch (Exception ex)
			{
				return (false, ex.Message);
			}
		}

		/// <summary>
		/// Comprehensive test of SMTP configuration.
		/// </summary>
		public async Task<(bool success, string message)> TestConfigurationAsync()
		{
			if (!_settings.Enabled)
				return (true, "Email service is disabled");

			var validationErrors = _settings.Validate();
			if (validationErrors.Length > 0)
			{
				foreach (var error in validationErrors)
					Log.Error($"Config error: {error}");
				return (false, $"Configuration errors: {string.Join(", ", validationErrors)}");
			}

			var (canConnect, connectError) = await TestSmtpConnectionAsync();
			if (!canConnect)
				return (false, $"Cannot connect to SMTP server: {connectError}");

			return (true, "SMTP configuration appears valid. Send a test email to fully verify.");
		}

		/// <summary>
		/// Sends a test email to verify the configuration works.
		/// </summary>
		public async Task<(bool success, string message)> SendTestEmailAsync(string toEmail)
		{
			var subject = $"[{_settings.SenderName}] Test Email";
			var body = $"This is a test email from {_settings.SenderName}.\n\nIf you received this, your email configuration is working correctly!\n\nSent at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC";

			var success = await SendEmailAsync(toEmail, subject, body);
			return success
				? (true, $"Test email sent successfully to {toEmail}")
				: (false, "Failed to send test email. Check the logs for details.");
		}
	}

	/// <summary>
	/// Email configuration settings.
	/// </summary>
	public class EmailSettings
	{
		public bool Enabled { get; set; } = false;
		public string SmtpHost { get; set; } = "smtp.gmail.com";
		public int SmtpPort { get; set; } = 587;
		public string SmtpUsername { get; set; } = "";
		public string SmtpPassword { get; set; } = "";
		public bool UseSsl { get; set; } = true;
		public string SenderEmail { get; set; } = "noreply@example.com";
		public string SenderName { get; set; } = "Melia Server";
		public string BaseUrl { get; set; } = "http://localhost";

		public static EmailSettings CreateDefault() => new EmailSettings();

		/// <summary>
		/// Validates the settings and returns any validation errors.
		/// </summary>
		public string[] Validate()
		{
			var errors = new System.Collections.Generic.List<string>();

			if (this.Enabled)
			{
				if (string.IsNullOrWhiteSpace(this.SmtpHost))
					errors.Add("SMTP host is required when email is enabled.");

				if (this.SmtpPort <= 0 || this.SmtpPort > 65535)
					errors.Add("SMTP port must be between 1 and 65535.");

				if (string.IsNullOrWhiteSpace(this.SmtpUsername))
					errors.Add("SMTP username is required when email is enabled.");

				if (string.IsNullOrWhiteSpace(this.SmtpPassword))
					errors.Add("SMTP password is required when email is enabled.");

				if (string.IsNullOrWhiteSpace(this.SenderEmail))
					errors.Add("Sender email is required when email is enabled.");

				try
				{
					var _ = new MailAddress(this.SenderEmail);
				}
				catch
				{
					errors.Add("Sender email is not a valid email address.");
				}

				if (string.IsNullOrWhiteSpace(this.BaseUrl))
					errors.Add("Base URL is required for generating password reset links.");

				if (!Uri.TryCreate(this.BaseUrl, UriKind.Absolute, out _))
					errors.Add("Base URL is not a valid URL.");
			}

			return errors.ToArray();
		}
	}
}
