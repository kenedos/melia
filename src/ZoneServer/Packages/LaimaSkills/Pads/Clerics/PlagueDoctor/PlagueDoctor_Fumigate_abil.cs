using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for the Fumigate: Purification pad, which protects the allies
	/// standing in it.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.PlagueDoctor_Fumigate_abil)]
	public class PlagueDoctor_Fumigate_abilOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Fumigate_Buff_ResAbil);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;

			if (initiator != creator && !initiator.IsAlly(creator))
				return;

			if (!creator.TryGetActiveAbilityLevel(AbilityId.PlagueDoctor6, out var abilityLevel))
				return;

			initiator.StartBuff(BuffId.Fumigate_Buff_ResAbil, abilityLevel, 0, pad.Trigger.RemainingLifeTime, creator, pad.Skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.Fumigate_Buff_ResAbil);
		}
	}
}
