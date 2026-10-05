using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper Broom Trap pad (rope_pad).
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.rope_pad)]
	public class rope_padOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const int UpdateIntervalMs = 100;
		private const int CircleDamageIntervalMs = 1000;
		private const int PadLifetimeMs = 120000;
		private const float RotationAnglePerUpdate = 13f;
		private const float InitialRotation = 40f;
		private const float CenterCircleRadius = 20f;
		private const string CircleTimerKey = "Melia.BroomTrap.CircleTimer";

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetBladedFanRange(3, 90, 15);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadLifetimeMs);
			pad.Trigger.MaxUseCount = 100000;
			pad.Trigger.MaxActorCount = 8;

			var trap = (Mob)PadCreateMonster(pad, "skill_sapper_trap2", pad.Position, 0f, 0, 0f, "HitProof#YES", "None", 1, true, "None", "None", true, "SCR_INIT_SAPPER_TRAP");
			if (trap != null)
			{
				trap.MonsterType = RelationType.Friendly;
				trap.Faction = FactionType.Law;
				trap.SetHittable(false);
				trap.StartBuff(BuffId.Invincible);
			}
			PadSetJumpRope(pad, "I_laser013", 1, 80, 0, 3, 1, 40, 3, 10);

			if (pad.Trigger.Area is BladedFan fan)
			{
				fan.Rotate(InitialRotation);
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (pad.IsDead || skill == null)
				return;

			if (!creator.IsEnemy(initiator))
				return;

			var skillHitResult = SCR_SkillHit(creator, initiator, skill);
			initiator.TakeDamage(skillHitResult.Damage, creator);

			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);
			Send.ZC_HIT_INFO(creator, skillHit.Target, skillHit.HitInfo);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (pad.Trigger.Area is BladedFan fan)
			{
				fan.Rotate(RotationAnglePerUpdate);
			}

			if (pad.IsDead || skill == null)
				return;

			var timer = pad.Variables.GetFloat(CircleTimerKey) + UpdateIntervalMs;
			if (timer < CircleDamageIntervalMs)
			{
				pad.Variables.SetFloat(CircleTimerKey, timer);
				return;
			}
			pad.Variables.SetFloat(CircleTimerKey, 0f);

			var splashArea = new Circle(pad.Position, CenterCircleRadius);
			var targets = creator.Map.GetAttackableEntitiesIn(creator, splashArea).Take(8);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(creator, target, skill);
				target.TakeDamage(skillHitResult.Damage, creator);

				var skillHit = new SkillHitInfo(creator, target, skill, skillHitResult);
				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);
		}
	}
}
