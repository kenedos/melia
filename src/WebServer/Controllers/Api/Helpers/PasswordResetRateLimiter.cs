using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Melia.Web.Controllers.Api.Helpers
{
	/// <summary>
	/// Specialized rate limiter for password reset requests.
	/// Tracks both IP addresses and email addresses to prevent abuse.
	/// </summary>
	public class PasswordResetRateLimiter
	{
		private readonly ConcurrentDictionary<string, RateLimitEntry> _ipLimits = new();
		private readonly ConcurrentDictionary<string, RateLimitEntry> _emailLimits = new();
		private readonly ConcurrentDictionary<string, DateTime> _emailCooldowns = new();

		private readonly int _maxRequestsPerIp;
		private readonly TimeSpan _ipWindowDuration;
		private readonly int _maxRequestsPerEmail;
		private readonly TimeSpan _emailWindowDuration;
		private readonly TimeSpan _emailCooldown;

		private readonly Timer _cleanupTimer;

		/// <summary>
		/// Creates a new PasswordResetRateLimiter with configurable limits.
		/// </summary>
		/// <param name="maxRequestsPerIp">Maximum requests allowed per IP in the time window.</param>
		/// <param name="ipWindowMinutes">Time window for IP rate limiting in minutes.</param>
		/// <param name="maxRequestsPerEmail">Maximum requests allowed per email in the time window.</param>
		/// <param name="emailWindowMinutes">Time window for email rate limiting in minutes.</param>
		/// <param name="emailCooldownMinutes">Minimum time between requests for the same email.</param>
		public PasswordResetRateLimiter(
			int maxRequestsPerIp = 5,
			int ipWindowMinutes = 15,
			int maxRequestsPerEmail = 3,
			int emailWindowMinutes = 60,
			int emailCooldownMinutes = 2)
		{
			_maxRequestsPerIp = maxRequestsPerIp;
			_ipWindowDuration = TimeSpan.FromMinutes(ipWindowMinutes);
			_maxRequestsPerEmail = maxRequestsPerEmail;
			_emailWindowDuration = TimeSpan.FromMinutes(emailWindowMinutes);
			_emailCooldown = TimeSpan.FromMinutes(emailCooldownMinutes);

			_cleanupTimer = new Timer(CleanupOldEntries, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
		}

		/// <summary>
		/// Checks if a password reset request should be allowed.
		/// </summary>
		/// <param name="ipAddress">The requester's IP address.</param>
		/// <param name="email">The email address for the reset request.</param>
		/// <returns>A result indicating if the request is allowed and why if not.</returns>
		public RateLimitResult CheckRequest(string ipAddress, string email)
		{
			var now = DateTime.UtcNow;
			var normalizedEmail = email?.ToLowerInvariant()?.Trim() ?? "";
			var normalizedIp = ipAddress ?? "";

			if (!string.IsNullOrEmpty(normalizedEmail))
			{
				if (_emailCooldowns.TryGetValue(normalizedEmail, out var lastRequest))
				{
					var timeSinceLastRequest = now - lastRequest;
					if (timeSinceLastRequest < _emailCooldown)
					{
						var waitTime = _emailCooldown - timeSinceLastRequest;
						return RateLimitResult.Blocked(
							$"Please wait {(int)waitTime.TotalSeconds} seconds before requesting another reset.",
							waitTime,
							RateLimitType.EmailCooldown
						);
					}
				}
			}

			if (!string.IsNullOrEmpty(normalizedEmail))
			{
				var emailEntry = _emailLimits.GetOrAdd(normalizedEmail, _ => new RateLimitEntry());
				emailEntry.CleanupOldRequests(_emailWindowDuration);

				if (emailEntry.RequestCount >= _maxRequestsPerEmail)
				{
					var oldestRequest = emailEntry.GetOldestRequestTime();
					var waitTime = oldestRequest.Add(_emailWindowDuration) - now;

					return RateLimitResult.Blocked(
						$"Too many reset requests for this email. Try again in {(int)waitTime.TotalMinutes} minutes.",
						waitTime,
						RateLimitType.EmailLimit
					);
				}
			}

			if (!string.IsNullOrEmpty(normalizedIp))
			{
				var ipEntry = _ipLimits.GetOrAdd(normalizedIp, _ => new RateLimitEntry());
				ipEntry.CleanupOldRequests(_ipWindowDuration);

				if (ipEntry.RequestCount >= _maxRequestsPerIp)
				{
					var oldestRequest = ipEntry.GetOldestRequestTime();
					var waitTime = oldestRequest.Add(_ipWindowDuration) - now;

					return RateLimitResult.Blocked(
						$"Too many requests from your IP address. Try again in {(int)waitTime.TotalMinutes} minutes.",
						waitTime,
						RateLimitType.IpLimit
					);
				}
			}

			return RateLimitResult.Allowed();
		}

		/// <summary>
		/// Records a successful password reset request.
		/// Call this after the request has been processed.
		/// </summary>
		public void RecordRequest(string ipAddress, string email)
		{
			var now = DateTime.UtcNow;
			var normalizedEmail = email?.ToLowerInvariant()?.Trim() ?? "";
			var normalizedIp = ipAddress ?? "";

			if (!string.IsNullOrEmpty(normalizedIp))
			{
				var ipEntry = _ipLimits.GetOrAdd(normalizedIp, _ => new RateLimitEntry());
				ipEntry.AddRequest(now);
			}

			if (!string.IsNullOrEmpty(normalizedEmail))
			{
				var emailEntry = _emailLimits.GetOrAdd(normalizedEmail, _ => new RateLimitEntry());
				emailEntry.AddRequest(now);
				_emailCooldowns[normalizedEmail] = now;
			}
		}

		/// <summary>
		/// Gets the remaining cooldown time for an email address.
		/// </summary>
		public TimeSpan? GetEmailCooldownRemaining(string email)
		{
			var normalizedEmail = email?.ToLowerInvariant()?.Trim() ?? "";

			if (_emailCooldowns.TryGetValue(normalizedEmail, out var lastRequest))
			{
				var timeSinceLastRequest = DateTime.UtcNow - lastRequest;
				if (timeSinceLastRequest < _emailCooldown)
					return _emailCooldown - timeSinceLastRequest;
			}

			return null;
		}

		private void CleanupOldEntries(object state)
		{
			var now = DateTime.UtcNow;

			foreach (var kvp in _ipLimits.ToArray())
			{
				kvp.Value.CleanupOldRequests(_ipWindowDuration);
				if (kvp.Value.RequestCount == 0)
					_ipLimits.TryRemove(kvp.Key, out _);
			}

			foreach (var kvp in _emailLimits.ToArray())
			{
				kvp.Value.CleanupOldRequests(_emailWindowDuration);
				if (kvp.Value.RequestCount == 0)
					_emailLimits.TryRemove(kvp.Key, out _);
			}

			foreach (var kvp in _emailCooldowns.ToArray())
			{
				if (now - kvp.Value > _emailCooldown)
					_emailCooldowns.TryRemove(kvp.Key, out _);
			}
		}

		private class RateLimitEntry
		{
			private readonly ConcurrentQueue<DateTime> _requestTimes = new();
			private readonly object _lock = new();

			public int RequestCount => _requestTimes.Count;

			public void AddRequest(DateTime time)
			{
				_requestTimes.Enqueue(time);
			}

			public DateTime GetOldestRequestTime()
			{
				return _requestTimes.TryPeek(out var time) ? time : DateTime.UtcNow;
			}

			public void CleanupOldRequests(TimeSpan windowDuration)
			{
				var cutoff = DateTime.UtcNow - windowDuration;

				lock (_lock)
				{
					while (_requestTimes.TryPeek(out var time) && time < cutoff)
						_requestTimes.TryDequeue(out _);
				}
			}
		}
	}

	/// <summary>
	/// Result of a rate limit check.
	/// </summary>
	public class RateLimitResult
	{
		public bool IsAllowed { get; private set; }
		public string Message { get; private set; }
		public TimeSpan? RetryAfter { get; private set; }
		public RateLimitType LimitType { get; private set; }

		private RateLimitResult() { }

		public static RateLimitResult Allowed()
		{
			return new RateLimitResult
			{
				IsAllowed = true,
				LimitType = RateLimitType.None
			};
		}

		public static RateLimitResult Blocked(string message, TimeSpan retryAfter, RateLimitType limitType)
		{
			return new RateLimitResult
			{
				IsAllowed = false,
				Message = message,
				RetryAfter = retryAfter,
				LimitType = limitType
			};
		}
	}

	/// <summary>
	/// Type of rate limit that was triggered.
	/// </summary>
	public enum RateLimitType
	{
		None,
		IpLimit,
		EmailLimit,
		EmailCooldown
	}
}
