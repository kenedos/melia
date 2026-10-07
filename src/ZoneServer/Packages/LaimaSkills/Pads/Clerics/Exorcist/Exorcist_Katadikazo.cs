using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the [Arts] Katadikazo: Flame Spear pad, the fire the
	/// spear leaves behind, which burns up to 7 enemies for 10% of
	/// Katadikazo every 0.5 seconds for 5 seconds.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Exorcist_Katadikazo)]
	public class Exorcist_KatadikazoOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int MaxTargets = 7;
		private const int DamageInterval = 500;
		private const float DamageRate = 0.1f;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(DamageInterval);
			pad.Trigger.LifeTime = LifeTime;
			pad.Trigger.MaxActorCount = MaxTargets;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			PadDamageEnemy(args.Trigger, DamageRate);
		}
	}
}
