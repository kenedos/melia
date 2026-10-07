using System;
using System.Collections.Generic;
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
	/// Handler for the Bullet Marker skill Napalm Bullet, a fiery blast in
	/// front of the Bullet Marker that, during Outrage, also tases the
	/// enemies it hits.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Bulletmarker_NapalmBullet)]
	public class Bulletmarker_NapalmBulletOverride : IForceSkillHandler
	{
		private const int HitCount = 4;
		private const float Length = 130f;
		private const float Width = 45f;
		private const float TaseDamageRate = 0.055f;
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(200);
		private static readonly TimeSpan TaseDuration = TimeSpan.FromSeconds(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target != null)
				caster.TurnTowards(target);

			var outrage = BulletMarkerSkillHelper.TryConsumeOutrage(caster);
			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var hits = new List<SkillHitInfo>();

			foreach (var hitTarget in caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill))
			{
				var modifier = BulletMarkerSkillHelper.CreateModifier(caster);
				modifier.HitCount = HitCount;

				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, modifier);
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, AniTime, TimeSpan.Zero));

				if (outrage && skillHitResult.Damage > 0)
					hitTarget.StartBuff(BuffId.Tase_Debuff, skill.Level, skillHitResult.Damage * TaseDamageRate, TaseDuration, caster, skill.Id);
			}

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, hits);
		}
	}
}
