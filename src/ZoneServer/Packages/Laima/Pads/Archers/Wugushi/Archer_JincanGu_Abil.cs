using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Wugushi
{
	[Package("laima")]
	[PadHandler(PadName.Archer_JincanGu_Abil)]
	public class Archer_JincanGu_AbilOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const int MaxTargets = 4;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(100f);
			pad.SetUpdateInterval(750);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);
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

			if (!creator.IsEnemy(initiator))
				return;

			if (initiator.IsBuffActive(BuffId.JincanGu_Abil_Debuff))
				return;

			var damage = (int)SCR_SkillHit(creator, initiator, skill).Damage;
			if (damage <= 0)
				return;

			AddPadBuff(creator, initiator, pad, BuffId.JincanGu_Abil_Debuff, skill.Level, damage, 60000, 1, 100);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var targets = pad.Trigger.GetAttackableEntities(creator)
				.OrderBy(t => t.IsBuffActive(BuffId.JincanGu_Abil_Debuff) ? 1 : 0)
				.Take(MaxTargets);

			foreach (var target in targets)
			{
				if (target.IsBuffActive(BuffId.JincanGu_Abil_Debuff))
					continue;

				var damage = (int)SCR_SkillHit(creator, target, skill).Damage;
				if (damage <= 0)
					continue;

				AddPadBuff(creator, target, pad, BuffId.JincanGu_Abil_Debuff, skill.Level, damage, 60000, 1, 100);
			}
		}
	}
}
