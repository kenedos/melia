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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;


namespace Melia.Zone.Pads.HandlersOverride.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper Spring Trap pad.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Sapper_SpringTrap)]
	public class Sapper_SpringTrapOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float PadRange = 15f;
		private const int UpdateIntervalMs = 100;
		private const int PadLifetimeMs = 120000;
		private const int DebuffDurationMs = 1000;
		private const int TrapMaxHP = 10;
		private const float ReactivationDelayMs = 1000f;
		private const string RevealedKey = "Melia.SpringTrap.Revealed";
		private const string CooldownKey = "Melia.SpringTrap.Cooldown";

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(PadRange);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadLifetimeMs);
			pad.Variables.SetBool(RevealedKey, false);
			pad.Variables.SetFloat(CooldownKey, 0f);

			var trap = (Mob)PadAttachMonster(pad, "pcskill_spring_trap", pad.Position, 0, 0, 0, 0, "HitProof#YES", "None", 1, true, "None", "None", true, "SCR_INIT_SAPPER_TRAP");
			if (trap != null)
			{
				var propertyOverrides = new PropertyOverrides();
				propertyOverrides.Add(PropertyName.HPCount, TrapMaxHP);
				trap.ApplyOverrides(propertyOverrides);
				trap.Properties.InvalidateAll();
				trap.HealToFull();
				trap.StartBuff(BuffId.Cover_Buff, TimeSpan.FromMinutes(2));
				trap.Died += (mob, killer) => pad.Destroy();
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad?.Monster?.Kill(null);
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

			if (pad.Variables.GetFloat(CooldownKey) > 0f)
				return;

			pad.Variables.SetFloat(CooldownKey, ReactivationDelayMs);

			if (!pad.Variables.GetBool(RevealedKey))
			{
				pad.Variables.SetBool(RevealedKey, true);
				if (pad.Monster != null)
				{
					pad.Monster.StopBuff(BuffId.Cover_Buff);
					Send.ZC_NORMAL.PlayAnimationOnEffect_6D(pad.Monster, pad.Handle);
				}
			}

			var hits = new List<SkillHitInfo>();

			var skillHitResult = SCR_SkillHit(creator, initiator, skill);
			initiator.TakeDamage(skillHitResult.Damage, creator);

			var skillHit = new SkillHitInfo(creator, initiator, skill, skillHitResult);

			if (skillHitResult.Damage > 0)
			{
				if (initiator.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(initiator, HitType.KnockBack, 120, 10, pad.Direction);
					skillHit.HitInfo.Type = HitType.KnockBack;
					initiator.ApplyKnockback(creator, skill, skillHit);
				}

				initiator.StartBuff(BuffId.SpringTrap_Debuff, TimeSpan.FromMilliseconds(DebuffDurationMs), creator);
			}

			hits.Add(skillHit);

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);

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
