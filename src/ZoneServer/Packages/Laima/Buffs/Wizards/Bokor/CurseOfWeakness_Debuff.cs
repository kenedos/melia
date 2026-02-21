using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Curse of Weakness debuff, applied by Bokor_Hexing.
	/// Decreases physical and magical attack, and reduces healing received.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CurseOfWeakness_Debuff)]
	public class CurseOfWeakness_DebuffOverride : BuffHandler
	{
		/// <summary>
		/// Called when the buff is activated.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;
			var skillLevel = buff.NumArg1;

			// Base reduction: 20% + 2% * SkillLv
			var baseReduction = 0.20f + (0.02f * skillLevel);


			var abilityMultiplier = 1f;
			// Bokor31 ability: 0.05% multiplier per ability level
			if (buff.Caster is Character character)
			{
				if (caster != null && character.TryGetActiveAbilityLevel(AbilityId.Bokor31, out var abilityLevel))
					abilityMultiplier += abilityLevel * 0.0005f;
			}

			// Calculate final reduction, capped at 90%
			var attackReductionRate = Math.Min(0.90f, baseReduction * abilityMultiplier);

			// Get current PATK and MATK values
			var currentPAtk = target.Properties.GetFloat(PropertyName.PATK);
			var currentMAtk = target.Properties.GetFloat(PropertyName.MATK);

			// Calculate the flat reduction based on current attack values
			var patkReduction = currentPAtk * attackReductionRate;
			var matkReduction = currentMAtk * attackReductionRate;

			// Apply flat reductions
			AddPropertyModifier(buff, target, PropertyName.PATK_BM, -patkReduction);
			AddPropertyModifier(buff, target, PropertyName.MATK_BM, -matkReduction);
		}

		/// <summary>
		/// Called when the buff ends.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove property modifiers
			RemovePropertyModifier(buff, target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_BM);
		}
	}
}
