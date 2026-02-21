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
using Yggdrasil.Scheduling;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_New_Aukuras)]
	public class Krivis_AukurasOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int PadLifeTimeSeconds = 300;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(150f);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(PadLifeTimeSeconds);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
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
			initiator.StartBuff(BuffId.Aukuras_Buff, skill.Level, hpRegenAmount, TimeSpan.FromMilliseconds(5000f), creator);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
			var abilityLevel = pad.NumArg2;

			pad.Position = creator.Position;

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
				target.StartBuff(BuffId.Aukuras_Buff, skill.Level, hpRegenAmount, TimeSpan.FromMilliseconds(5000f), creator);
			}
		}

		/// <summary>
		/// Returns the healing power of the skill.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float GetHPRegenAmount(ICombatEntity caster, ICombatEntity target, Skill skill, int abilityLevel)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var hpRegenAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			hpRegenAmount *= 1f + abilityLevel * 0.005f;

			return hpRegenAmount;
		}
	}
}
