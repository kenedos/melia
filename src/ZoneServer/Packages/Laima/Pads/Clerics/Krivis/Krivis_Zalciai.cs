using System;
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
	[PadHandler(PadName.Cleric_Zalciai)]
	public class Krivis_ZalciaiOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int BuffDurationMilliseconds = 4000;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = args.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(40f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(30000);
			pad.Trigger.MaxUseCount = 3 + skill.Level;
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

			if (!initiator.IsAlly(creator))
				return;

			if (initiator.IsBuffActive(BuffId.Zalciai_Buff))
				return;

			var amount = this.CalculateHeal(creator, initiator, skill);
			initiator.StartBuff(BuffId.Zalciai_Buff, skill.Level, amount, TimeSpan.FromMilliseconds(BuffDurationMilliseconds), creator);

			pad.Trigger.IncreaseUseCount();
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var targets = pad.Trigger.GetAlliedEntities(creator);
			if (pad.Trigger.Area.IsInside(creator.Position))
				targets.Add(creator);

			foreach (var target in targets)
			{
				if (target.IsBuffActive(BuffId.Zalciai_Buff))
					continue;

				if (target.IsDead)
					continue;

				var shieldHp = this.CalculateHeal(creator, target, skill);
				target.StartBuff(BuffId.Zalciai_Buff, skill.Level, shieldHp, TimeSpan.FromMilliseconds(BuffDurationMilliseconds), creator);

				pad.Trigger.IncreaseUseCount();
			}
		}

		/// <summary>
		/// Returns the HP of the shield
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float CalculateHeal(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var amount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);
			return amount;
		}
	}
}
