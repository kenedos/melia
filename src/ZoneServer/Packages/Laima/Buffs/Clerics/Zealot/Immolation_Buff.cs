using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.Immolation_Buff)]
	public class Immolation_BuffOverride : BuffHandler
	{
		private const float SkillRange = 100f;
		private const float HpLossRate = 0.01f;
		private const float MinimumCriticalChance = 0.20f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private const int UpdateIntervalMilliseconds = 1000;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;
		private static readonly TimeSpan MeltArmorDuration = TimeSpan.FromSeconds(10);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			RemovePropertyModifier(buff, character, PropertyName.ResFire_BM);
			RemovePropertyModifier(buff, character, PropertyName.Fire_Atk_BM);

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var fireResistanceBonus = skillLevel * 30f;
			var fireAttackBonus = 0f;

			if (character.TryGetActiveAbilityLevel(AbilityId.Zealot1, out var fireAttackLevel))
				fireAttackBonus = Math.Clamp(fireAttackLevel, 1, 10) * 100f;

			if (character.TryGetActiveAbilityLevel(AbilityId.Zealot4, out var fireResistanceLevel))
				fireResistanceBonus += Math.Clamp(fireResistanceLevel, 1, 5) * 300f;

			AddPropertyModifier(buff, character, PropertyName.ResFire_BM, fireResistanceBonus);

			if (fireAttackBonus > 0f)
				AddPropertyModifier(buff, character, PropertyName.Fire_Atk_BM, fireAttackBonus);

			buff.SetUpdateTime(UpdateIntervalMilliseconds);

			if (!character.IsDead && character.Map != null)
				this.DealAreaDamage(buff, character);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character || character.IsDead || character.Map == null)
			{
				buff.Target?.StopBuff(BuffId.Immolation_Buff);
				return;
			}

			this.ConsumeHp(character);
			this.DealAreaDamage(buff, character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target == null)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.ResFire_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.Fire_Atk_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Immolation_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Immolation_Buff, out _))
				return;

			modifier.MinCritChance += MinimumCriticalChance;
		}

		private void ConsumeHp(Character character)
		{
			var maximumHp = Math.Max(0f, character.Properties.GetFloat(PropertyName.MHP));
			var currentHp = Math.Max(0f, character.Properties.GetFloat(PropertyName.HP));
			var hpLoss = Math.Min(maximumHp * HpLossRate, Math.Max(0f, currentHp - 1f));

			if (hpLoss > 0f)
				character.ModifyHp(-hpLoss);
		}

		private void DealAreaDamage(Buff buff, Character caster)
		{
			if (caster.IsDead || caster.Map == null)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);

			if (!caster.TryGetSkill(SkillId.Zealot_Immolation, out var skill))
				skill = new Skill(caster, SkillId.Zealot_Immolation, skillLevel);

			var area = new CircleF(caster.Position, SkillRange);
			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.OfType<Mob>()
				.Where(target => !target.IsDead)
				.ToList();

			if (targets.Count == 0)
				return;

			var enhanceMultiplier = this.GetEnhanceMultiplier(caster);
			var hits = targets.Select(target =>
			{
				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= enhanceMultiplier;

				var result = SCR_SkillHit(caster, target, skill, modifier);

				if (result.Result != HitResultType.Dodge && result.Damage > 0)
					target.TakeDamage(result.Damage, caster);

				if (result.Result != HitResultType.Dodge && caster.TryGetActiveAbilityLevel(AbilityId.Zealot9, out var meltArmorLevel))
					target.StartBuff(BuffId.ImmolationMeltArmor_Debuff, Math.Clamp(meltArmorLevel, 1, 5), 0, MeltArmorDuration, caster, skill.Id);

				return new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero);
			}).ToArray();

			if (hits.Length > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private float GetEnhanceMultiplier(Character caster)
		{
			var abilityLevel = Math.Min(caster.Abilities.GetLevel(AbilityId.Zealot2), MaximumEnhanceLevel);

			if (abilityLevel <= 0)
				return 1f;

			var enhancePercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhancePercent += 10f;

			return 1f + enhancePercent / 100f;
		}
	}
}
