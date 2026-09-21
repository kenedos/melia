using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.Handlers.Archers.Arquebusier.Arquebusier_DesperateDefense;

namespace Melia.Zone.Pads.Handlers.Archers.Arquebusier
{
	/// <summary>
	/// Handler for the Desperate Defense barrier pad.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Arquebusier_DesperateDefense)]
	public class Arquebusier_DesperateDefensePad : ICreatePadHandler, IDestroyPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const string FlagMonsterClassName = "pcskill_DesperateDefense";
		private const int BuffDurationMs = 30000;

		/// <summary>
		/// Called when the pad is created.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetRange(BarrierRadius);
			Send.ZC_NORMAL.PadUpdate(pad, true);

			PadCreateMonster(pad, FlagMonsterClassName, pad.Position, creator.Direction.DegreeAngle, 0, "None", 1f);
		}

		/// <summary>
		/// Called when the pad is destroyed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.DesperateDefense_Buff);

			if (creator is Character character)
				Send.ZC_SEND_PC_EXPROP(character, new MsgParameter(BarrierSetProp, 0));

			Send.ZC_NORMAL.PadUpdate(pad, false);

			// The cooldown only starts counting once the barrier is gone
			skill?.StartCooldown(skill.Data.CooldownTime);
		}

		/// <summary>
		/// Called when an actor leaves the pad.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.DesperateDefense_Buff, false);
		}

		/// <summary>
		/// Called in regular intervals while the pad is active.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			if (creator == null || creator.IsDead || !creator.Position.InRange2D(pad.Position, BarrierRadius))
			{
				pad.Destroy();
				return;
			}

			PadBuff(pad, RelationType.Party, 0, 0, BuffId.DesperateDefense_Buff, 0, 0, BuffDurationMs, 1, 100, target => !target.IsBuffActive(BuffId.DesperateDefense_Buff));
		}
	}
}
