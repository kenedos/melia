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
	/// Handler for the Nak Muay skill Khao Loi, a flying knee kick.
	/// </summary>
	/// <remarks>
	/// [Arts] Khao Loi: Khao Trong teleports onto the targeted enemy and
	/// stuns it for 1.5 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.NakMuay_KhaoLoi)]
	public class NakMuay_KhaoLoiOverride : IGroundSkillHandler
	{
		private const int HitCount = 2;
		private const float Length = 65f;
		private const float Width = 35f;
		private const float LeapDistance = 15f;
		private const float TeleportRange = 150f;
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(450);
		private static readonly TimeSpan StunDuration = TimeSpan.FromMilliseconds(1500);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!NakMuaySkillHelper.CheckRamMuay(caster))
				return;

			var khaoTrong = caster.IsAbilityActive(AbilityId.NakMuay13);
			if (khaoTrong && (target == null || target.IsDead || !caster.Position.InRange2D(target.Position, TeleportRange)))
			{
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			NakMuaySkillHelper.ReduceMuayThaiCooldown(caster);

			if (khaoTrong)
			{
				var destination = target.Position.GetRelative(target.Position.GetDirection(caster.Position), LeapDistance);
				if (!caster.Map.Ground.TryGetNearestValidPosition(destination, out destination))
					destination = target.Position;

				caster.SetPosition(destination);
				caster.TurnTowards(target);
			}

			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var hits = new List<SkillHitInfo>();

			foreach (var hitTarget in caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, SkillModifier.MultiHit(HitCount));
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, AniTime, TimeSpan.Zero));

				if (khaoTrong && skillHitResult.Damage > 0)
					hitTarget.StartBuff(BuffId.Stun, 1, 0, StunDuration, caster, skill.Id);
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, hits);
		}
	}
}
