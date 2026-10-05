using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Zealot
{
	[Package("laima")]
	[BuffHandler(BuffId.FanaticIllusion_Buff)]
	public class FanaticIllusion_BuffOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumTargets = 6;
		private const int MaximumEnhanceLevel = 100;
		private const int UpdateIntervalMilliseconds = 1000;
		private const float DamageRange = 60f;
		private const float AccuracyBonus = 0.10f;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var resistancePercent = skillLevel * 10f;
			var currentResistance = Math.Max(0f, character.Properties.GetFloat(PropertyName.ResLightning));
			var resistanceBonus = currentResistance * resistancePercent / 100f;

			AddPropertyModifier(buff, character, PropertyName.ResLightning_BM, resistanceBonus);

			buff.SetUpdateTime(UpdateIntervalMilliseconds);

			if (!character.IsDead && character.Map != null)
				this.DealAreaDamage(buff, character);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character || character.IsDead || character.Map == null)
			{
				buff.Target?.StopBuff(BuffId.FanaticIllusion_Buff);
				return;
			}

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var spCostPerSecond = (int)MathF.Round(15f - (skillLevel - 1) * 10f / 9f);

			if (!character.TrySpendSp(spCostPerSecond))
			{
				character.StopBuff(BuffId.FanaticIllusion_Buff);
				return;
			}

			this.DealAreaDamage(buff, character);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.ResLightning_BM);
		}

		private void DealAreaDamage(Buff buff, Character caster)
		{
			if (caster.IsDead || caster.Map == null)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);

			if (!caster.TryGetSkill(SkillId.Zealot_FanaticIllusion, out var skill))
				skill = new Skill(caster, SkillId.Zealot_FanaticIllusion, skillLevel);

			var area = new CircleF(caster.Position, DamageRange);
			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => caster.Position.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();

			if (targets.Count == 0)
				return;

			var enhanceMultiplier = this.GetEnhanceMultiplier(caster);
			var hits = targets.Select(target =>
			{
				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= enhanceMultiplier;

				if (caster.IsAbilityActive(AbilityId.Zealot6))
					modifier.HitRateMultiplier += AccuracyBonus;

				var result = SCR_SkillHit(caster, target, skill, modifier);

				if (result.Result != HitResultType.Dodge)
					target.TakeDamage(result.Damage, caster);

				return new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero);
			}).ToArray();

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private float GetEnhanceMultiplier(Character caster)
		{
			var abilityLevel = Math.Min(caster.Abilities.GetLevel(AbilityId.Zealot3), MaximumEnhanceLevel);

			if (abilityLevel <= 0)
				return 1f;

			var enhancePercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhancePercent += 10f;

			return 1f + enhancePercent / 100f;
		}
	}
}
