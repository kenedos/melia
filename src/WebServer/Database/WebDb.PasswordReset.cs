using System;
using Yggdrasil.Logging;
using Yggdrasil.Security.Hashing;

namespace Melia.Web.Database
{
	public partial class WebDb
	{
		/// <summary>
		/// Creates a new password reset token for the given email.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <param name="tokenHash">The hashed reset token.</param>
		/// <param name="expiresAt">When the token expires.</param>
		/// <returns>True if the token was created successfully.</returns>
		public bool CreatePasswordResetToken(string email, string tokenHash, DateTime expiresAt)
		{
			try
			{
				using var conn = this.GetConnection();
				using var cmd = conn.CreateCommand();

				cmd.CommandText = @"
					INSERT INTO password_reset_tokens (email, token_hash, expires_at)
					VALUES (@email, @tokenHash, @expiresAt)";

				cmd.Parameters.AddWithValue("@email", email.ToLowerInvariant());
				cmd.Parameters.AddWithValue("@tokenHash", tokenHash);
				cmd.Parameters.AddWithValue("@expiresAt", expiresAt);

				return cmd.ExecuteNonQuery() > 0;
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to create password reset token: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Gets the active (unused, non-expired) token for an email.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <returns>A tuple of (tokenHash, expiresAt), or (null, default) if no valid token.</returns>
		public (string? tokenHash, DateTime expiresAt) GetActiveToken(string email)
		{
			try
			{
				using var conn = this.GetConnection();
				using var cmd = conn.CreateCommand();

				cmd.CommandText = @"
					SELECT token_hash, expires_at
					FROM password_reset_tokens
					WHERE email = @email
					  AND used_at IS NULL
					  AND expires_at > @now
					ORDER BY created_at DESC
					LIMIT 1";

				cmd.Parameters.AddWithValue("@email", email.ToLowerInvariant());
				cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);

				using var reader = cmd.ExecuteReader();
				if (reader.Read())
					return (reader.GetString(0), reader.GetDateTime(1));

				return (null, default);
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to get active token: {ex.Message}");
				return (null, default);
			}
		}

		/// <summary>
		/// Marks all tokens for an email as used.
		/// </summary>
		/// <param name="email">The email address.</param>
		public void MarkTokenAsUsed(string email)
		{
			try
			{
				using var conn = this.GetConnection();
				using var cmd = conn.CreateCommand();

				cmd.CommandText = @"
					UPDATE password_reset_tokens
					SET used_at = @now
					WHERE email = @email AND used_at IS NULL";

				cmd.Parameters.AddWithValue("@email", email.ToLowerInvariant());
				cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);

				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to mark token as used: {ex.Message}");
			}
		}

		/// <summary>
		/// Invalidates all existing password reset tokens for an email.
		/// Call this before creating a new token to ensure only one valid token exists.
		/// </summary>
		/// <param name="email">The email address.</param>
		public void InvalidatePasswordResetTokens(string email)
		{
			try
			{
				using var conn = this.GetConnection();
				using var cmd = conn.CreateCommand();

				cmd.CommandText = @"
					UPDATE password_reset_tokens
					SET used_at = @now
					WHERE email = @email AND used_at IS NULL";

				cmd.Parameters.AddWithValue("@email", email.ToLowerInvariant());
				cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);

				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to invalidate tokens: {ex.Message}");
			}
		}

		/// <summary>
		/// Updates the password for an account identified by email.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <param name="newPassword">The new password (will be hashed).</param>
		/// <returns>True if the password was updated successfully.</returns>
		public bool UpdateAccountPassword(string email, string newPassword)
		{
			try
			{
				using var conn = this.GetConnection();
				using var cmd = conn.CreateCommand();

				var hashedPassword = BCrypt.HashPassword(newPassword, BCrypt.GenerateSalt());

				cmd.CommandText = @"
					UPDATE accounts
					SET password = @password
					WHERE email = @email";

				cmd.Parameters.AddWithValue("@email", email.ToLowerInvariant());
				cmd.Parameters.AddWithValue("@password", hashedPassword);

				return cmd.ExecuteNonQuery() > 0;
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to update account password: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Cleans up expired password reset tokens.
		/// </summary>
		/// <returns>The number of tokens deleted.</returns>
		public int CleanupExpiredTokens()
		{
			try
			{
				using var conn = this.GetConnection();
				using var cmd = conn.CreateCommand();

				cmd.CommandText = @"
					DELETE FROM password_reset_tokens
					WHERE expires_at < @now OR used_at IS NOT NULL";

				cmd.Parameters.AddWithValue("@now", DateTime.UtcNow.AddDays(-1));

				return cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to cleanup expired tokens: {ex.Message}");
				return 0;
			}
		}
	}
}
