using System.Text.Json.Serialization;

namespace Melia.Web.Controllers.Api.Objects
{
	/// <summary>
	/// Request object for initiating a password reset.
	/// </summary>
	public class PasswordResetRequest
	{
		/// <summary>
		/// The email address associated with the account.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; } = "";
	}

	/// <summary>
	/// Request object for validating a password reset token.
	/// </summary>
	public class PasswordValidateRequest
	{
		/// <summary>
		/// The email address associated with the account.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; } = "";

		/// <summary>
		/// The reset token received via email.
		/// </summary>
		[JsonPropertyName("token")]
		public string Token { get; set; } = "";
	}

	/// <summary>
	/// Request object for confirming a password reset.
	/// </summary>
	public class PasswordConfirmRequest
	{
		/// <summary>
		/// The email address associated with the account.
		/// </summary>
		[JsonPropertyName("email")]
		public string Email { get; set; } = "";

		/// <summary>
		/// The reset token received via email.
		/// </summary>
		[JsonPropertyName("token")]
		public string Token { get; set; } = "";

		/// <summary>
		/// The new password to set.
		/// </summary>
		[JsonPropertyName("newPassword")]
		public string NewPassword { get; set; } = "";
	}
}
