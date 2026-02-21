using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Hunter
{
	/// <summary>
	/// Handle for the Pet_Heal buff, which restores a percentage of a
	/// companion's health over time and applies an after-effect buff.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Pet_Heal)]
	public class Pet_HealOverride : BuffHandler
	{
		private const float HealRate = 0.02f;
		private const string VarHealAmount = "Melia.Pet.HealAmount";
		private static readonly TimeSpan AfterBuffDuration = TimeSpan.FromMinutes(5);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var maxHp = buff.Target.Properties.GetFloat(PropertyName.MHP);
			var healAmount = (float)Math.Floor(maxHp * HealRate);
			buff.Vars.SetFloat(VarHealAmount, healAmount);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Vars.TryGetFloat(VarHealAmount, out var healAmount))
			{
				buff.Target.Heal(healAmount, 0);
			}
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.StartBuff(BuffId.Praise_Buff, buff.NumArg1, 0, AfterBuffDuration, buff.Target);
		}
	}
}
