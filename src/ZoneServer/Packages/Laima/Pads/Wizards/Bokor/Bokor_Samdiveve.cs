using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Wizards.Bokor
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Samdiveve)]
	public class Bokor_SamdiveveOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float PadRange = 120f;
		private const int PadLifeTimeMilliseconds = 30000;
		private const int BuffDuration = 300000;
		private const int UpdateIntervalMs = 1000;

		/// <summary>
		/// Handles the creation of the Samdiveve pad.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(PadRange);
			pad.SetUpdateInterval(UpdateIntervalMs);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(PadLifeTimeMilliseconds);

			this.CreateFlagMonster(pad);
		}

		/// <summary>
		/// Periodically refreshes the buff on allies currently inside the pad so
		/// the full 5-minute duration persists while they remain in the area.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			foreach (var ally in pad.Trigger.GetAlliedEntities(creator))
			{
				if (ally.IsDead)
					continue;

				ally.StartBuff(BuffId.Samdiveve_Buff, skill.Level, 0, TimeSpan.FromMilliseconds(BuffDuration), creator);
			}
		}

		/// <summary>
		/// Handles the destruction of the Samdiveve pad.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);
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
