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

namespace Melia.Zone.Pads.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for Hamaya's circle, which burns the enemies in it with Holy
	/// damage every second, double against devils, and lowers their critical
	/// resistance while they stand in it.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Miko_Hamaya)]
	public class Miko_HamayaOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float Range = 30f;
		private const int DamageInterval = 1000;
		private const float DevilDamageBonus = 1f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(DamageInterval);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(pad.Skill.Properties.GetFloat(PropertyName.CaptionRatio2));
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Hamaya_TakeDamage);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;

			if (args.Creator.IsEnemy(args.Initiator))
				args.Initiator.StartBuff(BuffId.Hamaya_TakeDamage, pad.Skill.Level, 0, pad.Trigger.RemainingLifeTime, args.Creator, pad.Skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.Hamaya_TakeDamage);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio3);
			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(maxTargets))
			{
				var modifier = new SkillModifier();
				if (target.Race == RaceType.Velnias)
					modifier.DamageMultiplier += DevilDamageBonus;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}

	/// <summary>
	/// Handler for the [Arts] Hamaya: Heal circle, which heals the party in it
	/// for 2 seconds.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Miko_Hamaya_Abil)]
	public class Miko_Hamaya_AbilOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler
	{
		private const float Range = 30f;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(2);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.Trigger.LifeTime = LifeTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Hamaya_Buff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			if (initiator == args.Creator || initiator.IsAlly(args.Creator))
				initiator.StartBuff(BuffId.Hamaya_Buff, pad.Skill.Level, 0, pad.Trigger.RemainingLifeTime, args.Creator, pad.Skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.Hamaya_Buff);
		}
	}
}
