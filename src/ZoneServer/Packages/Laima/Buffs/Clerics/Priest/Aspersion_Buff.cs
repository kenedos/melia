using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for Aspersion, Increases Defense.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Aspersion_Buff)]
	public class Aspersion_BuffOverride : BuffHandler
	{
		private const float BaseDEFMultiplier = 0.25f;
		private const float DEFMultiplierPerLevel = 0.05f;
		private const float AbilityBonus = 0.005f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = (ICombatEntity)buff.Caster;
			var target = buff.Target;
			var level = buff.NumArg1;
			var defMultiplier = BaseDEFMultiplier + (DEFMultiplierPerLevel * level);

			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Priest11, out var abilityLevel))
				byAbility += abilityLevel * AbilityBonus;
			defMultiplier *= byAbility;

			// Apply the defense modifier
			AddPropertyModifier(buff, target, PropertyName.DEF_RATE_BM, defMultiplier);
			AddPropertyModifier(buff, target, PropertyName.MDEF_RATE_BM, defMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove the defense modifier
			RemovePropertyModifier(buff, target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MDEF_RATE_BM);
		}
	}
}
