using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for Breaking Wheel's wheel, which strikes up to 10 enemies
	/// around it every 0.3 seconds for the skill's duration.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Inquisitor_BreakingWheel)]
	public class Inquisitor_BreakingWheelOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float Range = 45f;
		private const int UpdateInterval = 300;
		private const int MaxTargets = 10;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = pad.Skill.Properties.CaptionTime;

			var wheel = (Mob)PadCreateMonster(pad, "pcskill_Breaking_wheel", pad.Position, 0f, 0, 0f, "HitProof#YES", "None", 1, true, "None", "None", false, "None");
			if (wheel != null)
			{
				wheel.SetHittable(false);
				wheel.StartBuff(BuffId.Invincible);
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(MaxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
