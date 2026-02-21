using System.Threading.Tasks;

namespace Melia.Web.Services
{
	/// <summary>
	/// Defines a contract for a service that sends emails.
	/// </summary>
	public interface IEmailService
	{
		/// <summary>
		/// Sends a password reset email to a user.
		/// </summary>
		/// <param name="recipientEmail">The email address of the recipient.</param>
		/// <param name="resetToken">The unique token for the password reset link.</param>
		Task SendPasswordResetEmailAsync(string recipientEmail, string resetToken);
	}
}
