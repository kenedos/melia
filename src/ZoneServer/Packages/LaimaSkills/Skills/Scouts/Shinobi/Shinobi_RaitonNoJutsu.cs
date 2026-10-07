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

namespace Melia.Zone.Skills.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for the Shinobi skill Raiton no Jutsu, which releases the
	/// lightning gathered in the Shinobi's hands on the enemies ahead.
	/// </summary>
	/// <remarks>
	/// [Arts] Raiton no Jutsu: Hirai rushes to the target first.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Shinobi_Raiton_no_Jutsu)]
	public class Shinobi_RaitonNoJutsuOverride : IGroundSkillHandler
	{
		private const int HitCount = 3;
		private const float Length = 65f;
		private const float Width = 35f;
		private const float RushRange = 150f;
		private const float RushDistanceFromTarget = 20f;
		private static readonly TimeSpan ReleaseDelay = TimeSpan.FromMilliseconds(300);
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(200);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			if (caster.IsAbilityActive(AbilityId.Shinobi19) && target != null && !target.IsDead && caster.Position.InRange2D(target.Position, RushRange))
			{
				var destination = target.Position.GetRelative(target.Position.GetDirection(caster.Position), RushDistanceFromTarget);
				if (!caster.Map.Ground.TryGetNearestValidPosition(destination, out destination))
					destination = target.Position;

				caster.SetPosition(destination);
				caster.TurnTowards(target);
			}

			skill.Run(Release(skill, caster));
			ShinobiSkillHelper.ReplicateOnClones(caster, skill.Id, Release);
		}

		/// <summary>
		/// Releases the lightning on the enemies ahead of the attacker.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="attacker"></param>
		/// <returns></returns>
		private static async Task Release(Skill skill, ICombatEntity attacker)
		{
			await skill.Wait(ReleaseDelay);

			if (attacker.IsDead)
				return;

			var area = new Square(attacker.Position, attacker.Direction, Length, Width);
			var hits = new List<SkillHitInfo>();

			foreach (var target in attacker.Map.GetAttackableEnemiesIn(attacker, area).LimitBySDR(attacker, skill))
			{
				var skillHitResult = SCR_SkillHit(attacker, target, skill, SkillModifier.MultiHit(HitCount));
				target.TakeDamage(skillHitResult.Damage, attacker);

				hits.Add(new SkillHitInfo(attacker, target, skill, skillHitResult, AniTime, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(attacker, hits);
		}
	}
}
