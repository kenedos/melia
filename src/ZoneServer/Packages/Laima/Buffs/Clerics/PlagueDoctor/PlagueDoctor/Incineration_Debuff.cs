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
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handles Incineration periodic damage and Infect propagation.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Incineration_Debuff)]
	public class Incineration_DebuffOverride : BuffHandler, IDamageOverTimeBuffHandler
	{
		private const int MaximumEnhanceLevel = 100;
		private const int MaximumInfectLevel = 5;
		private const float MaximumDebuffDamageBonus = 0.50f;
		private const float DamageBonusPerDebuff = 0.10f;
		private const float BlackDeathSteamDamageBonusPerLevel = 0.05f;
		private const float InfectRadius = 100f;
		private const int NormalIntervalMilliseconds = 1000;
		private const int FastResponseIntervalMilliseconds = 800;
		private const string SpreadChainVariable = "Melia.PlagueDoctor.IncinerationSpreadChainId";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var interval = NormalIntervalMilliseconds;

			if (buff.Caster is Character caster && caster.IsAbilityActive(AbilityId.PlagueDoctor15))
				interval = FastResponseIntervalMilliseconds;

			buff.SetUpdateTime(interval);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.PlagueDoctor_Incineration, out var skill))
				return;

			var targetWasAlive = !buff.Target.IsDead;
			var remainingDuration = buff.RemainingDuration;
			var skillHitResult = SCR_SkillHit(caster, buff.Target, skill);
			var modifier = this.GetDamageMultiplier(buff, caster);

			skillHitResult.Damage *= modifier;
			buff.Target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, buff.Target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			skillHit.HitEffect = HitEffect.Impact;
			skillHit.ApplyDamage();

			Send.ZC_HIT_INFO(caster, buff.Target, skillHit.HitInfo);

			if (targetWasAlive && buff.Target.IsDead)
				this.TrySpreadIncineration(buff, caster, skill, remainingDuration);
		}

		private float GetDamageMultiplier(Buff buff, ICombatEntity caster)
		{
			var debuffCount = Math.Max((int)buff.NumArg2, 0);
			var debuffDamageBonus = Math.Min(debuffCount * DamageBonusPerDebuff, MaximumDebuffDamageBonus);
			var blackDeathSteamBonus = 0f;

			if (buff.Target.TryGetBuff(BuffId.PlagueVapours_Debuff, out var blackDeathSteam))
			{
				var blackDeathSteamLevel = Math.Clamp((int)blackDeathSteam.NumArg1, 1, 10);
				blackDeathSteamBonus = blackDeathSteamLevel * BlackDeathSteamDamageBonusPerLevel;
			}

			return (1f + debuffDamageBonus + blackDeathSteamBonus) * this.GetEnhanceMultiplier(caster);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character)
				return 1f;

			var enhanceLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.PlagueDoctor2), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}

		private void TrySpreadIncineration(Buff buff, ICombatEntity caster, Skill skill, TimeSpan remainingDuration)
		{
			if (caster is not Character character)
				return;

			if (!character.IsAbilityActive(AbilityId.PlagueDoctor13))
				return;

			if (remainingDuration <= TimeSpan.Zero)
				return;

			var spreadChainId = buff.Vars.GetInt(SpreadChainVariable);

			if (spreadChainId <= 0)
				return;

			var infectLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.PlagueDoctor13), 1, MaximumInfectLevel);
			var targets = buff.Target.Map.GetAttackableEnemiesInPosition(caster, buff.Target.Position, InfectRadius).Where(target => target != null && !target.IsDead && target != buff.Target && !target.IsBuffActive(BuffId.Incineration_Debuff)).OrderBy(target => buff.Target.Position.Get2DDistance(target.Position)).Take(infectLevel).ToList();

			foreach (var target in targets)
			{
				if (!PlagueDoctorSpreadTracker.TryConsume(spreadChainId))
					break;

				target.StartBuff(BuffId.Incineration_Debuff, buff.NumArg1, buff.NumArg2, remainingDuration, caster, skill.Id, newBuff => newBuff.Vars.SetInt(SpreadChainVariable, spreadChainId));
			}
		}
	}
}
