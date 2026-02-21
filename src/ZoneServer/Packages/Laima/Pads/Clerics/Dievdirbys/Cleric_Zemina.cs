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

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Zemina)]
	public class Cleric_ZeminaOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(100f);
			// Match statue lifetime: 15 + (skill.Level * 2) seconds
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(15 + skill.Level * 2);
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

			PadTargetBuffAfterBuffCheck(pad, initiator, RelationType.Party, 0, 0, BuffId.Ausirine_Buff, BuffId.CarveZemina_Buff, skill.Level, 0, 0, 1, 100, false);
			PadTargetBuffCheckAbility(pad, initiator, RelationType.Party, AbilityId.Dievdirbys33, 0, 0, BuffId.CarveZemina_Abil_Buff, skill.Level, 0, 0, 1, 100);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuffRemove(pad, initiator, RelationType.Party, 0, 0, BuffId.CarveZemina_Buff, false);
			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.CarveZemina_Abil_Buff, false);
		}
	}
}
