using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for Fanaticism: Martyr, which keeps the Zealot alive at 1 HP
	/// and lets them fall once it ends.
	/// </summary>
	/// <remarks>
	/// With [Arts] Martyr: Holy Sacrifice the fall heals nearby allies by
	/// half their max HP and grants them Price of Sacrifice.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Fanaticism_Martyrdom_Buff)]
	public class Zealot_Fanaticism_Martyrdom_BuffOverride : BuffHandler
	{
		private const float SacrificeRange = 200f;
		private const float SacrificeHealRate = 0.50f;
		private static readonly TimeSpan SacrificeDuration = TimeSpan.FromSeconds(60);

		public override void OnEnd(Buff buff)
		{
			var zealot = buff.Target;
			if (zealot.IsDead)
				return;

			if (zealot.IsAbilityActive(AbilityId.Zealot12))
			{
				foreach (var ally in PartySkillHelper.GetAlliesInRange(zealot, zealot.Position, SacrificeRange))
				{
					if (ally == zealot)
						continue;

					ally.Heal(ally.MaxHp * SacrificeHealRate, 0);
					ally.StartBuff(BuffId.Fanaticism_Zealot12_Buff, 1, 0, SacrificeDuration, zealot, SkillId.Zealot_Fanaticism);
				}
			}

			zealot.Kill(zealot);
		}
	}
}
