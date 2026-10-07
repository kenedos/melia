using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for Inquisitor: Burn's flame, which burns up to 3 enemies in it
	/// every 0.5 seconds with the skill that ignited it, and goes out after 5
	/// burns or 10 seconds.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Inquisitor_FireWall)]
	public class Inquisitor_FireWallOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float Range = 25f;
		private const int UpdateInterval = 500;
		private const int MaxTargetsPerTick = 3;
		private const int MaxBurns = 5;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(10);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = LifeTime;
			pad.Trigger.MaxUseCount = MaxBurns;
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

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(MaxTargetsPerTick))
			{
				var modifier = new SkillModifier();
				modifier.AttackAttribute = AttributeType.Fire;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

				if (pad.Trigger.IncreaseUseCount())
					break;
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
