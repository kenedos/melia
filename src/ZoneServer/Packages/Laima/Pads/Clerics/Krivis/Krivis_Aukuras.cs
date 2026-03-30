using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;
using Melia.Zone.World.Actors;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_New_Aukuras)]
	public class Krivis_AukurasOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const int PadLifeTimeSeconds = 300;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(150f);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(PadLifeTimeSeconds);
			pad.SetUpdateInterval(300);
			pad.FollowsTarget(creator);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Aukuras_Buff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
			var abilityLevel = pad.NumArg2;

			if (!initiator.IsAlly(creator))
				return;

			if (initiator.IsBuffActive(BuffId.Aukuras_Buff))
				return;

			var hpRegenAmount = this.GetHPRegenAmount(creator, initiator, skill, (int)abilityLevel);
			initiator.StartBuff(BuffId.Aukuras_Buff, skill.Level, hpRegenAmount, pad.Trigger.RemainingLifeTime, creator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
			var abilityLevel = pad.NumArg2;

			if (creator.IsDead)
			{
				pad.Destroy();
				return;
			}

			var targets = pad.Trigger.GetAlliedEntities(creator);
			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
			{
				if (target.IsDead)
					continue;

				if (target.IsBuffActive(BuffId.Aukuras_Buff))
					continue;

				var hpRegenAmount = this.GetHPRegenAmount(creator, target, skill, (int)abilityLevel);
				target.StartBuff(BuffId.Aukuras_Buff, skill.Level, hpRegenAmount, pad.Trigger.RemainingLifeTime, creator);
			}
		}

		private float GetHPRegenAmount(ICombatEntity caster, ICombatEntity target, Skill skill, int abilityLevel)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var hpRegenAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
			hpRegenAmount *= 1f + SCR_Get_AbilityReinforceRate(skill);

			return hpRegenAmount;
		}
	}
}
