using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for the Blossom Blader skill Flash, a lunge forward that
	/// pierces the enemies on the way 6 times.
	/// </summary>
	/// <remarks>
	/// Flash: Rush takes 5 seconds off the cooldown on a critical hit, at
	/// most once every 15 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.BlossomBlader_Flash)]
	public class BlossomBlader_FlashOverride : IGroundSkillHandler
	{
		private const int HitCount = 6;
		private const float LungeDistance = 100f;
		private const float Length = 150f;
		private const float Width = 30f;
		private const string RushTimeVar = "Melia.BlossomBlader.FlashRushTime";
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(150);
		private static readonly TimeSpan RushReduction = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan RushCooldown = TimeSpan.FromSeconds(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var startPos = caster.Position;
			var destination = caster.Map.Ground.GetLastValidPosition(startPos, startPos.GetRelative(caster.Direction, LungeDistance));
			var area = new Square(startPos, caster.Direction, Length, Width);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, destination);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, destination, ForceId.GetNew(), null);

			caster.SetPosition(destination);

			var hits = new List<SkillHitInfo>();
			var crit = false;

			foreach (var hitTarget in caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, hitTarget, skill, SkillModifier.MultiHit(HitCount));
				hitTarget.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, hitTarget, skill, skillHitResult, HitDelay, TimeSpan.Zero));

				if (skillHitResult.Damage > 0)
					BlossomBladerSkillHelper.ApplyFlowering(caster, hitTarget);

				crit |= skillHitResult.Result == HitResultType.Crit;
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			if (crit)
				this.TryRush(skill, caster);
		}

		/// <summary>
		/// With Flash: Rush, takes 5 seconds off Flash's cooldown, unless it
		/// did so in the last 15 seconds.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		private void TryRush(Skill skill, ICombatEntity caster)
		{
			if (!caster.IsAbilityActive(AbilityId.Blossomblader7))
				return;

			var now = GameClock.LocalNow;
			if (skill.Vars.TryGet<DateTime>(RushTimeVar, out var lastRush) && now - lastRush < RushCooldown)
				return;

			skill.Vars.Set(RushTimeVar, now);
			skill.ReduceCooldown(RushReduction);
		}
	}
}
