using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Sage
{
	[Package("laima")]
	[PadHandler(PadName.Sage_HoleOfDarkness)]
	public class Sage_HoleOfDarknessOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float PullRange = 50f;
		private const float PullDistance = 30f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(PullRange);
			pad.SetUpdateInterval(500);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(5000);
			pad.Trigger.MaxActorCount = 10;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
			var padCenter = pad.Position;

			var targets = pad.Trigger.GetAttackableEntities(creator);
			if (!targets.Any())
				return;

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				if (target == null || target.IsDead)
					continue;

				// Calculate damage
				var skillHitResult = SCR_SkillHit(creator, target, skill);
				var skillHit = new SkillHitInfo(creator, target, skill, skillHitResult);

				// Pull towards center using knockback
				if (target.IsKnockdownable())
				{
					var pullDirection = target.Position.GetDirection(padCenter);
					var pullFromPos = target.Position.GetRelative(pullDirection.Backwards, PullDistance);

					skillHit.KnockBackInfo = new KnockBackInfo(pullFromPos, target, HitType.KnockBack, 80, 10);
					skillHit.KnockBackInfo.Speed = 1;
					skillHit.KnockBackInfo.VPow = 1;
					skillHit.HitInfo.Type = HitType.KnockBack;

					target.ApplyKnockback(creator, skill, skillHit);
				}

				target.TakeDamage(skillHitResult.Damage, creator);
				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);
		}
	}
}
