using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using Melia.Web.Const;
using Melia.Web.Controllers.Api.Helpers;
using Melia.Web.Controllers.Api.Objects;
using Yggdrasil.Logging;

namespace Melia.Web.Controllers.Api
{
	/// <summary>
	/// Controller with API endpoints that provide account-related functions.
	/// </summary>
	internal class AccountController : JsonApiController
	{
		private static readonly IpRateLimiter AccountCreationLimiter = new(5, TimeSpan.FromMinutes(1));
		private static readonly IpRateLimiter PasswordResetLimiter = new(5, TimeSpan.FromHours(1));

		/// <summary>
		/// Handles requests to create a new account.
		/// </summary>
		/// <example>
		/// POST /account/create
		/// { "username": "myname", "password1": "mypassword", "password2": "mypassword" }
		/// </example>
		/// <returns></returns>
		[Route(HttpVerbs.Post, "/create")]
		public async Task Create()
		{
			if (!WebServer.Instance.Conf.Web.EnableApiAccountCreation)
			{
				await this.Error("Account creation via API is disabled.");
				return;
			}

			try
			{
				var request = await this.ParseJsonBody<CreateAccountRequest>();

				if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password1) || string.IsNullOrWhiteSpace(request.Password2))
				{
					await this.Error("The username and password must not be empty.");
					return;
				}

				if (request.Password1 != request.Password2)
				{
					await this.Error("The passwords do not match.");
					return;
				}

				if (request.Username.Length < 4)
				{
					await this.Error("The username must be at least 4 characters long.");
					return;
				}

				if (request.Password1.Length < 6)
				{
					await this.Error("The password must be at least 6 characters long.");
					return;
				}

				if (WebServer.Instance.Database.AccountExists(request.Username))
				{
					await this.Error("The account name already exists.");
					return;
				}

				if (AccountCreationLimiter.IsRateLimited(this.Request.RemoteEndPoint.Address.ToString()))
				{
					await this.Error("Too many account creation requests. Please try again later.");
					return;
				}

				if (!WebServer.Instance.Database.CreateAccount(request.Username, request.Password1))
				{
					await this.Error("Failed to create account. Please try again later.");
					return;
				}
			}
			catch (Exception ex)
			{
				Log.Warning("Failed to send kick message to coordinator. Error: {0}", ex);
				await this.Error("Request failed.");
				return;
			}

			await this.Ok(new
			{
				result = ApiResults.Success,
			});
		}

		/// <summary>
		/// Handles a request to initiate a password reset.
		/// Sends an email with a reset link if the email exists.
		/// </summary>
		/// <example>
		/// POST /account/password-reset/request
		/// { "email": "user@example.com" }
		/// </example>
		[Route(HttpVerbs.Post, "/password-reset/request")]
		public async Task RequestPasswordReset()
		{
			PasswordResetRequest request;
			try
			{
				request = await this.ParseJsonBody<PasswordResetRequest>();
			}
			catch
			{
				await this.Error("Invalid request format.");
				return;
			}

			var remoteIp = this.HttpContext.RemoteEndPoint.Address.ToString();

			if (PasswordResetLimiter.IsRateLimited(remoteIp))
			{
				await this.Error("Too many requests. Try again later.");
				return;
			}

			if (string.IsNullOrWhiteSpace(request.Email))
			{
				await this.Error("Email address is required.");
				return;
			}

			try
			{
				_ = new MailAddress(request.Email);
			}
			catch (FormatException)
			{
				await this.Error("Invalid email format.");
				return;
			}

			try
			{
				if (WebServer.Instance.Database.AccountExistsByEmail(request.Email))
				{
					WebServer.Instance.Database.InvalidatePasswordResetTokens(request.Email);

					var token = GenerateSecureToken();
					var tokenHash = HashToken(token);
					var expiration = DateTime.UtcNow.AddHours(1);

					if (WebServer.Instance.Database.CreatePasswordResetToken(request.Email, tokenHash, expiration))
					{
						var emailSent = await WebServer.Instance.EmailService.SendPasswordResetEmailAsync(request.Email, token);

						if (!emailSent)
							Log.Warning($"Failed to send password reset email to {request.Email}");
						else
							Log.Info($"Password reset email sent to {request.Email}");
					}
				}
				else
				{
					Log.Info($"Password reset requested for non-existent email: {request.Email}");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error processing password reset request: {ex}");
			}

			await this.Ok(new
			{
				result = ApiResults.Success,
				message = "If an account with that email exists, a password reset link has been sent."
			});
		}

		/// <summary>
		/// Validates a password reset token without changing the password.
		/// </summary>
		/// <example>
		/// POST /account/password-reset/validate
		/// { "email": "user@example.com", "token": "abc123..." }
		/// </example>
		[Route(HttpVerbs.Post, "/password-reset/validate")]
		public async Task ValidateResetToken()
		{
			PasswordValidateRequest request;
			try
			{
				request = await this.ParseJsonBody<PasswordValidateRequest>();
			}
			catch
			{
				await this.Error("Invalid request format.");
				return;
			}

			if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Token))
			{
				await this.Error("Email and token are required.");
				return;
			}

			var (storedHash, expiration) = WebServer.Instance.Database.GetActiveToken(request.Email);

			if (storedHash == null || expiration < DateTime.UtcNow)
			{
				await this.Error("Invalid or expired token.");
				return;
			}

			var submittedHash = HashToken(request.Token);
			if (!string.Equals(submittedHash, storedHash, StringComparison.Ordinal))
			{
				await this.Error("Invalid token.");
				return;
			}

			await this.Ok(new
			{
				result = ApiResults.Success,
				message = "Token is valid.",
				expiresAt = expiration.ToString("o")
			});
		}

		/// <summary>
		/// Handles the confirmation of a password reset using a token.
		/// </summary>
		/// <example>
		/// POST /account/password-reset/confirm
		/// { "email": "user@example.com", "token": "abc123...", "newPassword": "newSecurePassword123" }
		/// </example>
		[Route(HttpVerbs.Post, "/password-reset/confirm")]
		public async Task ConfirmPasswordReset()
		{
			PasswordConfirmRequest request;
			try
			{
				request = await this.ParseJsonBody<PasswordConfirmRequest>();
			}
			catch
			{
				await this.Error("Invalid request format.");
				return;
			}

			if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.NewPassword))
			{
				await this.Error("Email, token, and new password are required.");
				return;
			}

			if (request.NewPassword.Length < 6)
			{
				await this.Error("Password must be at least 6 characters long.");
				return;
			}

			var (storedHash, expiration) = WebServer.Instance.Database.GetActiveToken(request.Email);

			if (storedHash == null)
			{
				Log.Warning($"Password reset attempt with no active token for: {request.Email}");
				await this.Error("Invalid or expired token.");
				return;
			}

			if (expiration < DateTime.UtcNow)
			{
				Log.Warning($"Password reset attempt with expired token for: {request.Email}");
				await this.Error("Token has expired. Please request a new password reset.");
				return;
			}

			var submittedHash = HashToken(request.Token);
			if (!string.Equals(submittedHash, storedHash, StringComparison.Ordinal))
			{
				Log.Warning($"Password reset attempt with invalid token for: {request.Email}");
				await this.Error("Invalid token.");
				return;
			}

			if (!WebServer.Instance.Database.UpdateAccountPassword(request.Email, request.NewPassword))
			{
				Log.Error($"Failed to update password for: {request.Email}");
				await this.Error("Failed to update password. Please try again.");
				return;
			}

			WebServer.Instance.Database.MarkTokenAsUsed(request.Email);

			_ = Task.Run(async () =>
			{
				try
				{
					await WebServer.Instance.EmailService.SendPasswordResetConfirmationAsync(request.Email);
				}
				catch (Exception ex)
				{
					Log.Warning($"Failed to send password change confirmation to {request.Email}: {ex.Message}");
				}
			});

			Log.Info($"Password successfully reset for: {request.Email}");
			await this.Ok(new
			{
				result = ApiResults.Success,
				message = "Password updated successfully. You can now log in with your new password."
			});
		}

		/// <summary>
		/// Generates a cryptographically secure random token.
		/// </summary>
		private static string GenerateSecureToken()
		{
			using var rng = RandomNumberGenerator.Create();
			var bytes = new byte[32];
			rng.GetBytes(bytes);
			return Convert.ToBase64String(bytes)
				.Replace('+', '-')
				.Replace('/', '_')
				.TrimEnd('=');
		}

		/// <summary>
		/// Hashes a token using SHA256 for secure storage.
		/// </summary>
		private static string HashToken(string token)
		{
			var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
			return Convert.ToBase64String(hash);
		}
	}
}
