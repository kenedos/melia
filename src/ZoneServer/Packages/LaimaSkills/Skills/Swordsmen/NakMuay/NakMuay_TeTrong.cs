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

namespace Melia.Zone.Skills.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for the Nak Muay skill Te Trong, a powerful kick that spins
	/// the enemy and knocks it down.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.NakMuay_TeTrong)]
	public class NakMuay_TeTrongOverride : IGroundSkillHandler
	{
		private const int HitCount = 2;
		private const float Length = 55f;
		private const float Width = 30f;
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(300);
		private static readonly TimeSpan SpinDuration = TimeSpan.FromMilliseconds(1500);

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
			var hits = new List<SkillHitInfo>();

			foreach (var hitTarget in caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, SkillModifier.MultiHit(HitCount));
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, hitTarget, skill, skillHitResult, AniTime, TimeSpan.Zero);

				if (skillHitResult.Damage > 0 && hitTarget.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster, hitTarget, KnockBackType.KnockDown, 150, 60, KnockDirection.CasterForward);
					skillHit.HitInfo.KnockBackType = KnockBackType.KnockDown;
					hitTarget.ApplyKnockdown(caster, skill, skillHit);
					hitTarget.StartBuff(BuffId.TeTrong_Debuff, skill.Level, 0, SpinDuration, caster, skill.Id);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
