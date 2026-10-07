using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Grand Cross pad, which strikes up to 15 enemies on
	/// it every second for 5 seconds.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Exorcist_Koinonia)]
	public class Exorcist_KoinoniaOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int MaxTargets = 15;
		private const int DamageInterval = 1000;
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
			PadDamageEnemy(args.Trigger);
		}
	}
}
