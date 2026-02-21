using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Hunter
{
	/// <summary>
	/// Handler for the Howling debuff.
	/// Reduces target's evasion, accuracy, and critical defense.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Howling_Debuff)]
	public class Howling_DebuffOverride : BuffHandler
	{
		private const float BaseRate = 0.10f;
		private const float RatePerLevel = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;

			var byAbility = 1f;
			if (buff.Caster is ICombatEntity caster && caster.TryGetActiveAbilityLevel(AbilityId.Hunter14, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;

			var rate = (BaseRate + RatePerLevel * skillLevel) * byAbility;

			var reduceDR = buff.Target.Properties.GetFloat(PropertyName.DR) * rate;
			var reduceHR = buff.Target.Properties.GetFloat(PropertyName.HR) * rate;
			var reduceCRTDR = buff.Target.Properties.GetFloat(PropertyName.CRTDR) * rate;

			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, -reduceDR);
			AddPropertyModifier(buff, buff.Target, PropertyName.HR_BM, -reduceHR);
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -reduceCRTDR);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}
}
