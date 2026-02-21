using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Wizards.Bokor
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Samdiveve)]
	public class Bokor_SamdiveveOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler
	{
		private const float PadRange = 120f;
		private const int PadLifeTimeMilliseconds = 30000;
		private const int BuffDuration = 300000;

		/// <summary>
		/// Handles the creation of the Samdiveve pad.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(PadRange);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadLifeTimeMilliseconds);

			this.CreateFlagMonster(pad);
		}

		/// <summary>
		/// Handles the destruction of the Samdiveve pad.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		/// <summary>
		/// Handles an entity entering the Samdiveve pad area.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!initiator.IsAlly(creator))
				return;

			if (initiator.IsBuffActive(BuffId.Samdiveve_Buff))
				return;

			initiator.StartBuff(BuffId.Samdiveve_Buff, skill.Level, 0, TimeSpan.FromMilliseconds(BuffDuration), creator);
		}

		/// <summary>
		/// Creates the Ogouveve flag monster for the Samdiveve pad.
		/// </summary>
		private void CreateFlagMonster(Pad pad)
		{
			var monster = PadCreateMonster(pad, "Ogouveve_flag_skill", pad.Position, 0f, 0, PadLifeTimeMilliseconds, "", "None", 1, true, "None", "None", false, "SET_PVE_NODAMAGE");
			var mob = monster as Mob;
			mob.MonsterType = RelationType.Friendly;
			mob.Faction = FactionType.Neutral;
			mob.StartBuff(BuffId.Invincible);
		}
	}
}
