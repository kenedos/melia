using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Archers.Mergen
{
	/// <summary>
	/// Handler for Arrow Sprinkle's storm of arrows, which damages every
	/// enemy inside it at each volley.
	/// </summary>
	/// <remarks>
	/// The skill handler sets the lifetime from the cast time.
	/// </remarks>
	[Package("laima-skills")]
	[PadHandler(PadName.Mergen_ArrowRain)]
	public class Mergen_ArrowRainOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float RainRange = 60f;
		private const int VolleyInterval = 500;
		private static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(RainRange);
			pad.SetUpdateInterval(VolleyInterval);
			pad.Trigger.LifeTime = MaxDuration;
			pad.Trigger.MaxActorCount = int.MaxValue;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			PadDamageEnemy(args.Trigger);
		}
	}
}
