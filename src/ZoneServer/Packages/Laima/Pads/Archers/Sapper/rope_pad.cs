using System;
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
		private const int PadLifetimeMs = 120000;
		private const int TrapMaxHP = 10;
		private const float RotationAnglePerUpdate = 13f;
		private const float InitialRotation = 40f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetBladedFanRange(3, 90, 15);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadLifetimeMs);
			pad.Trigger.MaxUseCount = 100000;
			pad.Trigger.MaxActorCount = 8;

			var trap = (Mob)PadCreateMonster(pad, "skill_sapper_trap2", pad.Position, 0f, 0, 0f, "HitProof#YES", "None", 1, true, "None", "None", true, "SCR_INIT_SAPPER_TRAP");
			if (trap != null)
			{
				var propertyOverrides = new PropertyOverrides();
				propertyOverrides.Add(PropertyName.HPCount, TrapMaxHP);
				trap.ApplyOverrides(propertyOverrides);
				trap.Properties.InvalidateAll();
				trap.HealToFull();
				trap.Died += (mob, killer) => pad.Destroy();
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

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
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
		}
	}
}
