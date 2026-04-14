using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Melstis)]
	public class Krivis_MelstisOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		/// <summary>
		/// Initializes the Melstis pad.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(90f);
			pad.SetUpdateInterval(500);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(25);
		}

		/// <summary>
		/// Cleans up the Melstis pad and removes buffs from all targets.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		/// <summary>
		/// Grants Melstis_Buff to allies entering the pad.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!initiator.IsAlly(creator))
				return;

			if (initiator.IsBuffActive(BuffId.Melstis_Buff))
				return;

			initiator.StartBuff(BuffId.Melstis_Buff, skill.Level, 0, TimeSpan.FromSeconds(20), creator, skill.Id);
		}

		/// <summary>
		/// Periodically checks for allies inside the pad and grants
		/// Melstis_Buff to those who don't already have it.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

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

				if (target.IsBuffActive(BuffId.Melstis_Buff))
					continue;

				target.StartBuff(BuffId.Melstis_Buff, skill.Level, 0, TimeSpan.FromSeconds(20), creator, skill.Id);
			}
		}
	}
}
