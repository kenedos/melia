using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for Pandemic's Panic, which raises the damage the target
	/// takes from the Plague Doctor by 20% per debuff, up to 100%.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Panic_Pandemic_Debuff)]
	public class PlagueDoctor_Panic_Pandemic_DebuffOverride : BuffHandler
	{
		private const float DamageBonusPerDebuff = 0.20f;
		private const float MaxDamageBonus = 1f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Panic_Pandemic_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Panic_Pandemic_Debuff, out var buff) || buff.Caster != attacker)
				return;

			var debuffCount = PlagueDoctorSkillHelper.CountDebuffs(target);
			modifier.FinalDamageMultiplier += Math.Min(MaxDamageBonus, debuffCount * DamageBonusPerDebuff);
		}
	}
}
