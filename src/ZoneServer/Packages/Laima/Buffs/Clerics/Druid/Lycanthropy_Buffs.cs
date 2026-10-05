using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Druid;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Druid
{
	public abstract class LycanthropyStatBuffHandler : BuffHandler
	{
		private const int HealingIntervalMilliseconds = 8000;
		private const float HealingRate = 0.008f;

		protected void ActivateStats(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);
			var maximumHp = character.Properties.GetFloat(PropertyName.MHP);
			var physicalDefense = character.Properties.GetFloat(PropertyName.DEF);
			var magicDefense = character.Properties.GetFloat(PropertyName.MDEF);
			var criticalRate = character.Properties.GetFloat(PropertyName.CRTHR);
			var blockPenetration = character.Properties.GetFloat(PropertyName.BLK_BREAK);
			var criticalRateBonusRate = skillLevel * 0.04f;

			if (Druid_TransformationSpecialityAbility.IsActive(character))
				criticalRateBonusRate += 0.04f;

			AddPropertyModifier(buff, character, PropertyName.MHP_BM, maximumHp * 0.40f);
			AddPropertyModifier(buff, character, PropertyName.DEF_BM, physicalDefense * 0.20f);
			AddPropertyModifier(buff, character, PropertyName.MDEF_BM, magicDefense * 0.20f);
			AddPropertyModifier(buff, character, PropertyName.CRTHR_BM, criticalRate * criticalRateBonusRate);
			AddPropertyModifier(buff, character, PropertyName.BLK_BREAK_BM, blockPenetration * 0.04f);
			AddPropertyModifier(buff, character, PropertyName.MSPD_BM, 4f);

			if (Druid_TransformationSpecialityAbility.IsActive(character))
			{
				var evasion = character.Properties.GetFloat(PropertyName.DR);
				var hpRecovery = character.Properties.GetFloat(PropertyName.RHP);

				AddPropertyModifier(buff, character, PropertyName.DR_BM, evasion * 0.04f);
				AddPropertyModifier(buff, character, PropertyName.RHP_BM, hpRecovery * 0.04f);
			}

			buff.SetUpdateTime(HealingIntervalMilliseconds);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			var maximumHp = buff.Target.Properties.GetFloat(PropertyName.MHP);
			var healingAmount = maximumHp * HealingRate;

			if (healingAmount > 0)
				buff.Target.Heal(healingAmount, 0);
		}

		protected void DeactivateStats(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MHP_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BREAK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.RHP_BM);
		}

		protected void AddTemporarySkill(Buff buff, Character character, SkillId skillId, int level)
		{
			var variableName = $"Druid.Lycanthropy.Added.{(int)skillId}";

			if (character.Skills.Has(skillId))
				return;

			character.Skills.Add(new Skill(character, skillId, level));
			buff.Vars.SetInt(variableName, 1);
		}

		protected void RemoveTemporarySkill(Buff buff, Character character, SkillId skillId)
		{
			var variableName = $"Druid.Lycanthropy.Added.{(int)skillId}";

			if (buff.Vars.GetInt(variableName) == 0)
				return;

			character.Skills.Remove(skillId);

			if (character.Skills.Has(skillId))
			{
				character.Skills.RemoveSilent(skillId);
				Send.ZC_SKILL_REMOVE(character, skillId);
			}
		}
	}

	[Package("laima")]
	[BuffHandler(BuffId.Lycanthropy_Buff)]
	public class Lycanthropy_BuffOverride : LycanthropyStatBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);

			this.ActivateStats(buff);
			this.AddTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_1, skillLevel);
			this.AddTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_3, skillLevel);
			this.AddTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_4, skillLevel);
			this.AddTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_5, skillLevel);

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Mon_pcskill_boss_werewolf_Skill_1);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
			{
				this.RemoveTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_1);
				this.RemoveTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_3);
				this.RemoveTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_4);
				this.RemoveTemporarySkill(buff, character, SkillId.Mon_pcskill_boss_werewolf_Skill_5);
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
			}

			this.DeactivateStats(buff);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Lycanthropy_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult result)
		{
			if (!attacker.TryGetBuff(BuffId.Lycanthropy_Buff, out var buff))
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);
			modifier.DamageMultiplier *= 1f + skillLevel * 0.04f;
		}
	}

	[Package("laima")]
	[BuffHandler(BuffId.Lycanthropy_Half_Buff)]
	public class Lycanthropy_Half_BuffOverride : LycanthropyStatBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			this.ActivateStats(buff);
			this.AddTemporarySkill(buff, character, SkillId.Lycan_Half_Attack, 1);
			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Lycan_Half_Attack);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
			{
				this.RemoveTemporarySkill(buff, character, SkillId.Lycan_Half_Attack);
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
			}

			this.DeactivateStats(buff);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Lycanthropy_Half_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult result)
		{
			if (!attacker.TryGetBuff(BuffId.Lycanthropy_Half_Buff, out var buff))
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, 1, 10);
			modifier.DamageMultiplier *= 1f + skillLevel * 0.04f;
			modifier.FinalDamageMultiplier *= 1f + (4f + skillLevel * 0.20f) / 100f;
		}
	}
}
