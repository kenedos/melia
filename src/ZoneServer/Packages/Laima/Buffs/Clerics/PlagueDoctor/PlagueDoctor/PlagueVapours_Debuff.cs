using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handles Black Death Steam periodic damage, critical resistance reduction and contagion.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.PlagueVapours_Debuff)]
	public class PlagueVapours_DebuffOverride : BuffHandler, IDamageOverTimeBuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private const float CriticalResistanceReductionPerLevel = 0.02f;
		private const float ContagionChancePerLevel = 10f;
		private const float ContagionRadius = 80f;
		private const float DefaultIntervalMilliseconds = 1000f;
		private const float FastContagionIntervalMilliseconds = 900f;
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(15);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var reductionRate = skillLevel * CriticalResistanceReductionPerLevel;
			var criticalResistanceReduction = buff.Target.Properties.GetFloat(PropertyName.CRTDR) * reductionRate;

			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -criticalResistanceReduction);

			var interval = DefaultIntervalMilliseconds;

			if (buff.Caster is Character caster && caster.IsAbilityActive(AbilityId.PlagueDoctor16))
				interval = FastContagionIntervalMilliseconds;

			buff.SetUpdateTime((int)interval);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.PlagueDoctor_PlagueVapours, out var skill))
				return;

			this.ApplyPeriodicDamage(buff, caster, skill);
			this.TrySpread(buff, caster, skill);
		}

		private void ApplyPeriodicDamage(Buff buff, ICombatEntity caster, Skill skill)
		{
			var skillHitResult = SCR_SkillHit(caster, buff.Target, skill);
			skillHitResult.Damage *= this.GetEnhanceMultiplier(caster);

			var skillHit = new SkillHitInfo(caster, buff.Target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			skillHit.HitEffect = HitEffect.Impact;
			skillHit.ApplyDamage();

			Send.ZC_HIT_INFO(caster, buff.Target, skillHit.HitInfo);
		}

		private void TrySpread(Buff buff, ICombatEntity caster, Skill skill)
		{
			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var contagionChance = Math.Min(skillLevel * ContagionChancePerLevel, 100f);

			if (RandomProvider.Get().Next(100) >= contagionChance)
				return;

			var nearbyTarget = buff.Target.Map.GetAttackableEnemiesInPosition(caster, buff.Target.Position, ContagionRadius).Where(target => target != null && !target.IsDead && target != buff.Target && !target.IsBuffActive(BuffId.PlagueVapours_Debuff)).OrderBy(target => buff.Target.Position.Get2DDistance(target.Position)).FirstOrDefault();

			if (nearbyTarget == null)
				return;

			var spreadChainId = (int)buff.NumArg2;

			if (spreadChainId <= 0 || !PlagueDoctorSpreadTracker.TryConsume(spreadChainId))
				return;

			nearbyTarget.StartBuff(BuffId.PlagueVapours_Debuff, skillLevel, spreadChainId, DebuffDuration, caster, skill.Id);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 1f;

			var enhanceLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.PlagueDoctor9), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
