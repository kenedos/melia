using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Indulgentia buff.
	/// Recovers HP continuously while the buff is active.
	/// The amount of HP recovered increases by 10% when the Guardian Saint buff is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Indulgentia_Buff)]
	public class Pardoner_Indulgentia_BuffOverride : BuffHandler
	{
		private const float BaseHealFactor = 50f; // Base heal amount
		private const float PerLevelHealFactor = 25f; // Additional heal per skill level
		private const float GuardianSaintBonus = 0.10f; // 10% bonus when Guardian Saint is active
		private const float UpdateIntervalMs = 1000f; // Heal every 1 second

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Initial activation - no immediate heal, will heal on first update
		}

		public void OnUpdate(Buff buff)
		{
			var target = buff.Target;
			var caster = buff.Caster as ICombatEntity;

			if (target == null || target.IsDead)
				return;

			// Calculate heal amount based on skill level and caster's stats
			var skillLevel = buff.NumArg1;
			var healAmount = BaseHealFactor + (PerLevelHealFactor * skillLevel);

			// Add scaling from caster's SPR and INT if available
			if (caster != null)
			{
				var spr = caster.Properties.GetFloat(PropertyName.MNA);
				var intStat = caster.Properties.GetFloat(PropertyName.INT);
				healAmount += (spr * 0.5f) + (intStat * 0.3f);
			}

			// Apply Guardian Saint bonus if active
			if (target.IsBuffActive(BuffId.PatronSaint_Buff))
			{
				healAmount *= (1f + GuardianSaintBonus);
			}

			// Apply Indulgentia: Enhance attribute bonus
			if (caster != null && caster.TryGetActiveAbility(AbilityId.Pardoner1, out var enhanceAbility))
			{
				// 0.5% per level + 10% at max level
				var abilityBonus = enhanceAbility.Level * 0.005f;
				if (enhanceAbility.Level >= 100) // Assuming max level is 100
					abilityBonus += 0.10f;
				healAmount *= (1f + abilityBonus);
			}

			// Heal the target
			target.Heal((int)healAmount, 0);
		}

		public override void OnEnd(Buff buff)
		{
			// No cleanup needed
		}
	}
}
