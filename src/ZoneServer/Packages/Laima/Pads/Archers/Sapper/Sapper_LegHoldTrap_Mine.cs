using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper Leg Hold Trap mine pad.
	/// The trap persists after activation with a 5-second re-activation
	/// delay. Each mob can only be trapped once per pad lifetime.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Sapper_LegHoldTrap_Mine)]
	public class Sapper_LegHoldTrap_MineOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float PadRange = 25f;
		private const int UpdateIntervalMs = 750;
		private const int PadLifetimeMs = 120000;
		private const int TrapMaxHP = 10;
		private const float ReactivationDelayMs = 3000f;
		private const string CooldownKey = "Melia.LegHold.Cooldown";

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			pad.SetRange(PadRange);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadLifetimeMs);

			pad.Variables.SetFloat(CooldownKey, 0f);

			var trap = (Mob)PadCreateMonster(pad, "pcskill_leghold_trap", pad.Position, 0f, 0, 0f, "HitProof#YES", "None", 1, true, "None", "None", true, "SCR_INIT_SAPPER_TRAP");
			if (trap != null)
			{
				var propertyOverrides = new PropertyOverrides();
				propertyOverrides.Add(PropertyName.HPCount, TrapMaxHP);
				trap.ApplyOverrides(propertyOverrides);
				trap.Properties.InvalidateAll();
				trap.HealToFull();
				trap.Died += (mob, killer) => pad.Destroy();
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			if (pad.Variables.GetFloat(CooldownKey) > 0f)
				return;

			pad.Variables.SetFloat(CooldownKey, ReactivationDelayMs);

			var hits = new List<SkillHitInfo>();

			var skillHitResult = SCR_SkillHit(creator, initiator, skill);
			initiator.TakeDamage(skillHitResult.Damage, creator);

			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);
			hits.Add(skillHit);

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);

			if (skillHitResult.Damage > 0)
			{
				var debuffDurationMs = (int)((3 + 0.6 * skill.Level) * 1000);
				initiator.StartBuff(BuffId.LegHoldTrap_Debuff, TimeSpan.FromMilliseconds(debuffDurationMs), creator);
			}
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var cooldown = pad.Variables.GetFloat(CooldownKey);
			if (cooldown > 0f)
				pad.Variables.SetFloat(CooldownKey, Math.Max(0f, cooldown - UpdateIntervalMs));
		}
	}
}
