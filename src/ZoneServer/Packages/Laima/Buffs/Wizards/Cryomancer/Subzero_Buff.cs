using System;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizard.Cryomancer
{
	/// <summary>
	/// Handler for the Subzero buff, which changes the target's attribute to
	/// Ice/Water and has a chance to apply Cryomancer_Freeze debuff to
	/// attackers.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Subzero_Buff)]
	public class Subzero_BuffOverride : BuffHandler
	{
		private const float BaseFreezeChance = 20f;
		private const float FreezeChancePerLevel = 6f;

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.Subzero_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if ((skill.Data.Attribute == AttributeType.None) || (skill.Data.Attribute == AttributeType.Melee) || (skill.Data.Attribute == AttributeType.Magic))
				modifier.AttackAttribute = AttributeType.Ice;
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Subzero_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Subzero_Buff, out var buff))
				return;

			if (skill.Data.ClassType == SkillClassType.Magic)
				return;

			var freezeChance = BaseFreezeChance + (buff.NumArg1 * FreezeChancePerLevel);
			if (RandomProvider.Get().Next(100) < freezeChance)
			{
				var freezeDuration = buff.NumArg2;

				attacker.StartBuff(BuffId.Cryomancer_Freeze, buff.NumArg1, 0, TimeSpan.FromMilliseconds(freezeDuration), buff.Target);

				if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Cryomancer10, out var abilLevel)
					&& buff.Target.TryGetSkill(SkillId.Cryomancer_SubzeroShield, out var subzeroSkill))
				{
					var counterModifier = SkillModifier.Default;
					counterModifier.DamageMultiplier = 0.5f * abilLevel;

					var counterHitResult = SCR_SkillHit(buff.Target, attacker, subzeroSkill, counterModifier);
					attacker.TakeDamage(counterHitResult.Damage, buff.Target);

					var hitInfo = new HitInfo(buff.Target, attacker, counterHitResult.Damage, HitResultType.Hit);
					hitInfo.Type = HitType.Ice;
					Send.ZC_HIT_INFO(buff.Target, attacker, hitInfo);
				}
			}
		}
	}
}
