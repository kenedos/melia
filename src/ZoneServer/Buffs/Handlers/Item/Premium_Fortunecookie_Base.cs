using System;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Base handler for Fortunecookie buffs which have a shared max duration.
	/// </summary>
	public abstract class Premium_Fortunecookie_Base : BuffHandler
	{
		private static readonly TimeSpan MaxDuration = TimeSpan.FromMinutes(30);

		public override void WhileActive(Buff buff)
		{
			if (buff.RemainingDuration > MaxDuration)
			{
				buff.IncreaseDuration(MaxDuration);
			}
		}
	}
}
