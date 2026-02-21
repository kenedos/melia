using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Sadhu
{
	[Package("laima")]
	[PadHandler(PadName.Sadhu_Bind)]
	public class Sadhu_BindOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(40f);
			pad.SetUpdateInterval(750);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(99999);
			pad.Trigger.MaxUseCount = 5;
			pad.NumArg1 = skill.SkillAtkAdd;
			pad.Trigger.MaxActorCount = skill.Level * 1 + 4;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
			PadRemoveBuff(pad, RelationType.Enemy, 0, 0, BuffId.SadhuBind_Debuff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!PadActivate(pad, initiator, RelationType.Enemy)) return;
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.SadhuBind_Debuff, false);
			if (!PadDeactivate(pad, initiator, RelationType.Enemy)) return;
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadBuff(pad, RelationType.Enemy, 0, 0, BuffId.SadhuBind_Debuff, skill.Level, 0, 0, 1, 100);
			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
		}
	}
}
