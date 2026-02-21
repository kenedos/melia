using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;
using Melia.Shared.Data.Database;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_New_SafetyZone_abil)]
	public class Cleric_New_SafetyZone_abilOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const string ShieldVariableName = "Melia.SafetyZone.Shield.HP";

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = args.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(40f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(30000);

			var shieldHp = this.CalculateShieldHP(creator, creator, skill);
			pad.Variables.SetFloat(ShieldVariableName, shieldHp);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			var targets = pad.Trigger.GetAlliedEntities(creator);
			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);
			foreach (var target in targets)
				target.RemoveBuff(BuffId.SafetyZone_Buff);

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!initiator.IsAlly(creator))
				return;

			if (initiator.IsBuffActive(BuffId.SafetyZone_Buff))
				return;

			var remainingShieldHp = pad.Variables.GetFloat(ShieldVariableName);
			initiator.StartBuff(BuffId.SafetyZone_Buff, skill.Level, 0f, TimeSpan.Zero, creator);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var initiator = args.Initiator;
			initiator.RemoveBuff(BuffId.SafetyZone_Buff);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			var remainingShieldHp = pad.Variables.GetFloat(ShieldVariableName);
			if (remainingShieldHp <= 0)
			{
				pad.Destroy();
				return;
			}
		}

		private float CalculateShieldHP(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			return SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);
		}
	}
}
