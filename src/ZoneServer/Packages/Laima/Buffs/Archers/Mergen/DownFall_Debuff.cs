using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Packages.Laima.Skills.Archers.Mergen;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.Mergen
{
	[BuffHandler(BuffId.DownFall_Debuff)]
	public class DownFall_DebuffOverride : BuffHandler
	{
		private const float BroadenRange = 120;
		private const float BroadenDamageMultiplier = 0.50f;
		private static readonly TimeSpan NormalInterval = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan BroadenInterval = TimeSpan.FromMilliseconds(800);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.UpdateTime = this.HasBroaden(buff)
				? BroadenInterval
				: NormalInterval;
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;
			var caster = buff.Caster as ICombatEntity;

			if (target == null || target.IsDead || caster == null || caster.IsDead)
			{
				buff.Target?.RemoveBuff(buff.Id);
				return;
			}

			if (!caster.TryGetSkill(SkillId.Mergen_DownFall, out var skill))
			{
				buff.Target?.RemoveBuff(buff.Id);
				return;
			}

			this.DealDamage(caster, target, skill, 1f);

			if (this.HasBroaden(buff))
				this.DealBroadenDamage(caster, target, skill);
		}

		private void DealBroadenDamage(ICombatEntity caster, ICombatEntity originalTarget, Skill skill)
		{
			var aoeAttackRatio = caster.Properties.GetFloat(PropertyName.SR);
			var maximumTargets = 3 + Math.Max(0, (int)MathF.Floor(aoeAttackRatio / 3f));

			var nearbyTargets = originalTarget.Map
				.GetAttackableEnemiesInPosition(caster, originalTarget.Position, BroadenRange)
				.Where(target => target != null && !target.IsDead && target != originalTarget)
				.OrderBy(target => originalTarget.Position.Get2DDistance(target.Position))
				.Take(maximumTargets)
				.ToList();

			foreach (var nearbyTarget in nearbyTargets)
				this.DealDamage(caster, nearbyTarget, skill, BroadenDamageMultiplier);
		}

		private void DealDamage(ICombatEntity caster, ICombatEntity target, Skill skill, float damageMultiplier)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill);

			if (skillHitResult.Result == HitResultType.Dodge)
				return;

			var enhanceMultiplier = this.GetEnhanceMultiplier(caster);
			enhanceMultiplier *= MergenZenithHelper.GetDamageMultiplier(caster);
			skillHitResult.Damage = Math.Max(1, (int)(skillHitResult.Damage * enhanceMultiplier * damageMultiplier));

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);
			skillHit.ApplyDamage();
			Send.ZC_HIT_INFO(caster, target, skillHit.HitInfo);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character || !character.TryGetAbility(AbilityId.Mergen11, out var ability))
				return 1f;

			var level = Math.Clamp(ability.Level, 0, 100);
			var bonusPercent = level * 0.5f;

			if (level >= 100)
				bonusPercent += 10f;

			return 1f + (bonusPercent / 100f);
		}

		private bool HasBroaden(Buff buff)
		{
			return buff.Caster is Character character && character.IsAbilityActive(AbilityId.Mergen14);
		}
	}
}
