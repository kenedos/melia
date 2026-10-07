using System;
using System.Collections.Generic;
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

namespace Melia.Zone.Skills.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for the Bullet Marker skill Rest in Peace, two bursts of
	/// four shots down a line ahead, 55% stronger during Outrage.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Bulletmarker_RestInPeace)]
	public class Bulletmarker_RestInPeaceOverride : IGroundSkillHandler
	{
		private const int HitsPerBurst = 4;
		private const float Length = 150f;
		private const float Width = 30f;
		private const float OutrageDamageBonus = 0.55f;
		private static readonly (int HitDelay, int AniTime)[] Bursts = [(40, 0), (800, 600)];

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!BulletMarkerSkillHelper.CheckDoubleGunStance(caster))
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			var outrage = BulletMarkerSkillHelper.TryConsumeOutrage(caster);

			skill.Run(this.Fire(skill, caster, outrage));
		}

		/// <summary>
		/// Fires the two bursts down the line ahead of the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="outrage"></param>
		/// <returns></returns>
		private async Task Fire(Skill skill, ICombatEntity caster, bool outrage)
		{
			foreach (var burst in Bursts)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(burst.AniTime));

				if (caster.IsDead)
					return;

				var area = new Square(caster.Position, caster.Direction, Length, Width);
				var hits = new List<SkillHitInfo>();

				foreach (var target in caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill))
				{
					var modifier = BulletMarkerSkillHelper.CreateModifier(caster);
					modifier.HitCount = HitsPerBurst;

					if (outrage)
						modifier.FinalDamageMultiplier += OutrageDamageBonus;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(burst.HitDelay), TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
