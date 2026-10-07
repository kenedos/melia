using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Mon;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Pads.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for the shikigami orb's outward flight, which turns back
	/// where it stops unless the Soul Fox threw it.
	/// </summary>
	/// <remarks>
	/// The orb is only the visual; the throwing skill deals the damage.
	/// </remarks>
	[Package("laima-skills")]
	[PadHandler(PadName.Onmyoji_CrystalballShikigami_Pad)]
	public class Onmyoji_CrystalballShikigamiOverride : ICreatePadHandler, IDestroyPadHandler
	{
		public const float Range = 30f;
		public static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.Trigger.LifeTime = LifeTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);

			if (creator.IsDead || creator.Map != pad.Map || Mon_pcskill_FireFoxShikigami_Skill_1Override.IsNoReturnOrb(pad))
				return;

			var returnPad = new Pad(PadName.Onmyoji_CrystalballShikigami_Pad2, creator, pad.Skill, new Circle(pad.Position, Range));
			returnPad.Position = pad.Position;
			returnPad.Direction = pad.Direction;
			pad.Map.AddPad(returnPad);
		}
	}

	/// <summary>
	/// Handler for the shikigami orb's return flight, which homes back on its
	/// creator.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Onmyoji_CrystalballShikigami_Pad2)]
	public class Onmyoji_CrystalballShikigami_ReturnOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int UpdateInterval = 100;
		private const float ReturnSpeed = 350f;
		private const float ArrivalDistance = 10f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Onmyoji_CrystalballShikigamiOverride.Range);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = Onmyoji_CrystalballShikigamiOverride.LifeTime;
			pad.Movement.Speed = ReturnSpeed;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			if (creator.IsDead || creator.Map != pad.Map || pad.Position.InRange2D(creator.Position, ArrivalDistance))
			{
				pad.Destroy();
				return;
			}

			var destination = pad.Map.Ground.GetLastValidPosition(pad.Position, creator.Position);
			if (pad.Position.InRange2D(destination, ArrivalDistance))
			{
				pad.Destroy();
				return;
			}

			pad.Movement.MoveTo(destination);
		}
	}
}
