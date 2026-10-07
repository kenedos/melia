using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Templar skill Mortal Wave, which spends the
	/// Templar's Uplift for extra damage and a longer reach.
	/// </summary>
	/// <remarks>
	/// With Mortal Wave: Jab the wave hits once and a single target, and
	/// gains 50% damage per Uplift stage instead.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Templer_MortalWave)]
	public class Templer_MortalWaveOverride : IGroundSkillHandler
	{
		private const int HitCount = 7;
		private const float WaveLength = 40f;
		private const float WaveWidth = 25f;
		private const float UpliftDamageRate = 0.50f;
		private const float JabDamageRatePerUplift = 0.50f;
		private static readonly float[] UpliftLengthMultipliers = [1f, 1f, 2f, 3f];
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(600);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var uplift = TemplarSkillHelper.ConsumeUplift(caster);
			var isJab = caster.IsAbilityActive(AbilityId.Templar19);
			var length = isJab ? WaveLength : WaveLength * UpliftLengthMultipliers[uplift];

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: length, width: WaveWidth, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, splashArea, uplift, isJab));
		}

		/// <summary>
		/// Strikes the targets along the wave.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		/// <param name="uplift"></param>
		/// <param name="isJab"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea, int uplift, bool isJab)
		{
			await skill.Wait(HitDelay);

			var hits = new List<SkillHitInfo>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hitTargets = isJab ? targets.Take(1) : targets.LimitBySDR(caster, skill);

			foreach (var target in hitTargets)
			{
				var modifier = SkillModifier.MultiHit(isJab ? 1 : HitCount);

				if (isJab)
					modifier.DamageMultiplier += uplift * JabDamageRatePerUplift;
				else if (uplift > 0)
					modifier.DamageMultiplier += UpliftDamageRate;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
