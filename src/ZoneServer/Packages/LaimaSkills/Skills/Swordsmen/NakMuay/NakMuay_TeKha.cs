using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace Melia.Zone.Skills.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for the Nak Muay skill Te Kha, a low kick that roots the
	/// enemy for 1.5 seconds and deals 50% more to small enemies and bosses.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.NakMuay_TeKha)]
	public class NakMuay_TeKhaOverride : IGroundSkillHandler
	{
		private const int HitsPerKick = 2;
		private const float Length = 55f;
		private const float Width = 30f;
		private const float BonusDamage = 0.5f;
		private static readonly (int Time, int AniTime)[] HitTimings = [(100, 300), (200, 400)];
		private static readonly TimeSpan RootDuration = TimeSpan.FromMilliseconds(1500);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!NakMuaySkillHelper.CheckRamMuay(caster))
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			NakMuaySkillHelper.ReduceMuayThaiCooldown(caster);

			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill).ToList();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, targets));
		}

		/// <summary>
		/// Strikes the targets once per kick, then roots the ones still standing.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private async Task Attack(Skill skill, ICombatEntity caster, List<ICombatEntity> targets)
		{
			var elapsed = 0;

			foreach (var timing in HitTimings)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(timing.Time - elapsed));
				elapsed = timing.Time;

				if (caster.IsDead)
					return;

				var aniTime = TimeSpan.FromMilliseconds(timing.AniTime - timing.Time);
				var hits = new List<SkillHitInfo>();

				foreach (var hitTarget in targets.Where(t => !t.IsDead))
				{
					var modifier = SkillModifier.MultiHit(HitsPerKick);
					if (hitTarget.EffectiveSize == SizeType.S || hitTarget.Rank == MonsterRank.Boss)
						modifier.DamageMultiplier += BonusDamage;

					var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, modifier);
					hitTarget.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, aniTime, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}

			foreach (var hitTarget in targets.Where(t => !t.IsDead))
				hitTarget.StartBuff(BuffId.TeKha_Debuff, skill.Level, 0, RootDuration, caster, skill.Id);
		}
	}
}
