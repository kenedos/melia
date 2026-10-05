using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Abilities.Handlers.Archers.Cannoneer;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.Cannoneer
{
	[Package("laima")]
	[BuffHandler(BuffId.SmokeGrenade_Debuff)]
	public class Cannoneer_SmokeGrenade_DebuffOverride : BuffHandler
	{
		private const int MaximumSkillLevel = 10;
		private const float BaseReduction = 0.20f;
		private const float ReductionPerLevel = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, MaximumSkillLevel);
			var reduction = BaseReduction + skillLevel * ReductionPerLevel;
			var accuracyReduction = buff.Target.Properties.GetFloat(PropertyName.HR) * reduction;
			var evasionReduction = buff.Target.Properties.GetFloat(PropertyName.DR) * reduction;

			AddPropertyModifier(buff, buff.Target, PropertyName.HR_BM, -accuracyReduction);
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, -evasionReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.SmokeGrenade_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.SmokeGrenade_Debuff, out var buff))
				return;

			if (attacker != buff.Caster || attacker is not Character character)
				return;

			var damageMultiplier = Cannoneer_SmokeGrenadeAdditionalDamageAbility.GetDamageMultiplier(character);
			modifier.DamageMultiplier *= damageMultiplier;
		}
	}
}
