using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor
{
	public static class PlagueDoctorSpreadTracker
	{
		private const int MaximumSpreadTargets = 5;
		private static int _nextChainId;
		private static readonly ConcurrentDictionary<int, SpreadChain> Chains = new();

		public static int CreateChain()
		{
			var chainId = Interlocked.Increment(ref _nextChainId);
			Chains[chainId] = new SpreadChain(MaximumSpreadTargets, DateTime.UtcNow.AddMinutes(1));
			return chainId;
		}

		public static bool TryConsume(int chainId)
		{
			if (!Chains.TryGetValue(chainId, out var chain))
				return false;

			if (chain.ExpiresAt <= DateTime.UtcNow)
			{
				Chains.TryRemove(chainId, out _);
				return false;
			}

			while (true)
			{
				var remaining = Volatile.Read(ref chain.Remaining);

				if (remaining <= 0)
					return false;

				if (Interlocked.CompareExchange(ref chain.Remaining, remaining - 1, remaining) != remaining)
					continue;

				if (remaining - 1 <= 0)
					Chains.TryRemove(chainId, out _);

				return true;
			}
		}

		private sealed class SpreadChain
		{
			public int Remaining;
			public DateTime ExpiresAt { get; }

			public SpreadChain(int remaining, DateTime expiresAt)
			{
				this.Remaining = remaining;
				this.ExpiresAt = expiresAt;
			}
		}
	}
}
