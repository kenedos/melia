using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Oracle
{
	[Package("laima")]
	[PadHandler(PadName.Oracle_Foretell)]
	public class Oracle_ForetellOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float PadRadius = 80f;
		private const float DriftIntervalMs = 100f;
		private const float NearRange = 80f;
		private const float NearSpeedShare = 0.5f;
		private const float FarSpeedShare = 0.25f;
		private const float MinSpeed = 5f;
		private const float DriftDistance = 60f;
		private const float MinDriftDistance = 10f;

		private const string DriftDirectionVar = "Melia.Foretell.DriftDirection";

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			// One circle per caster: a re-cast replaces the one still drifting.
			foreach (var other in pad.Map.GetPads(p => p != pad && p.Name == pad.Name && p.Creator == pad.Creator))
				other.Destroy();

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(PadRadius);
			pad.SetUpdateInterval(DriftIntervalMs);
			pad.Trigger.LifeTime = pad.Skill.Properties.CaptionTime;

			// Taken once, because the circle holds the heading it was cast
			// on for the rest of its life.
			pad.Variables.Set(DriftDirectionVar, creator.Direction);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Foretell_Buff);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Foretell_Vibora_Buff);
			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (creator.IsBuffActive(BuffId.ITEM_BUFF_VIBORA_AURA_LV4))
				PadTargetBuff(pad, initiator, RelationType.Party, 0, 0, BuffId.Foretell_Vibora_Buff, skill.Level, 0, 0, 1, 100, false);
			else
				PadTargetBuff(pad, initiator, RelationType.Party, 0, 0, BuffId.Foretell_Buff, skill.Level, 0, 0, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Foretell_Buff, false);
			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Foretell_Vibora_Buff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			// Ramps and stairs leave the pad buried in the ground it drifted
			// up, because a move only interpolates between two heights.
			if (pad.Map.Ground.TryGetHeightAt(pad.Position, out var height))
				pad.Position = pad.Position.WithHeight(height);

			pad.Movement.Speed = DriftSpeed(pad, creator);

			var direction = pad.Variables.Get<Direction>(DriftDirectionVar);
			var ahead = pad.Position.GetRelative(direction, DriftDistance);
			var destination = pad.Map.Ground.GetLastValidPosition(pad.Position, ahead);

			// Nothing turns the circle, so a wall would hold it there for the
			// rest of its life. It ends instead of sitting in the geometry.
			if (destination.Get2DDistance(pad.Position) <= MinDriftDistance)
			{
				pad.Movement.Stop();
				pad.Destroy();

				return;
			}

			pad.Movement.MoveTo(destination);
		}

		/// <summary>
		/// Returns the speed the pad drifts at, which is half the caster's
		/// own speed while they stay with it and a quarter of it once they
		/// fall behind.
		/// </summary>
		/// <remarks>
		/// Solved in MSPD and returned in world units per second, which is
		/// what a movement component's speed is.
		/// </remarks>
		/// <param name="pad"></param>
		/// <param name="creator"></param>
		private static float DriftSpeed(Pad pad, ICombatEntity creator)
		{
			var casterSpeed = creator.Properties.GetFloat(PropertyName.MSPD);

			var speed = creator.Position.InRange2D(pad.Position, NearRange)
				? casterSpeed * NearSpeedShare
				: casterSpeed * FarSpeedShare;

			return Math.Max(MinSpeed, speed) * Movement.UnitsPerMspdSecond;
		}
	}
}
