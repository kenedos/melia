using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Appraiser
{
	/// <summary>
	/// Deals Triplet Lense damage once every second.
	/// NumArg1 contains the skill level.
	/// NumArg2 contains the number of consumed lenses.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.TripletLens_Debuff)]
	public class TripletLens_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const int MaximumEnhanceLevel = 100;
		private static readonly TimeSpan HitAnimationTime =
			TimeSpan.FromMilliseconds(100);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Caster is not Character caster || caster.IsDead)
				return;

			var skillLevel = Math.Max(1, (int)buff.NumArg1);

			Skill skill;

			if (!caster.TryGetSkill(
				SkillId.Appraiser_TripletLens,
				out skill
			))
			{
				skill = new Skill(
					caster,
					SkillId.Appraiser_TripletLens,
					skillLevel
				);
			}

			var modifier = SkillModifier.Default;
			modifier.DamageMultiplier *= this.GetEnhanceMultiplier(caster);

			var skillHitResult = SCR_SkillHit(
				caster,
				buff.Target,
				skill,
				modifier
			);

			buff.Target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(
				caster,
				buff.Target,
				skill,
				skillHitResult,
				HitAnimationTime,
				TimeSpan.Zero
			);

			Send.ZC_SKILL_HIT_INFO(
				caster,
				new[] { skillHit }
			);
		}

		private float GetEnhanceMultiplier(Character caster)
		{
			var abilityLevel = Math.Min(
				caster.Abilities.GetLevel(AbilityId.Appraiser13),
				MaximumEnhanceLevel
			);

			if (abilityLevel <= 0)
				return 1f;

			var enhancePercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhancePercent += 10f;

			return 1f + enhancePercent / 100f;
		}
	}
}
