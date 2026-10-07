using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Aqua Benedicta pad, a puddle of holy water that
	/// damages the enemies in it every 0.5 seconds for 7 seconds.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Exorcist_AquaBenedicta)]
	public class Exorcist_AquaBenedictaOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float Range = 50f;
		private const int DamageInterval = 500;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(7);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(DamageInterval);
			pad.Trigger.LifeTime = LifeTime;
			pad.Trigger.MaxActorCount = (int)pad.Skill.Properties.GetFloat(PropertyName.CaptionRatio);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.AquaBenedictaHIT_Debuff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;

			if (args.Creator.IsEnemy(args.Initiator))
				args.Initiator.StartBuff(BuffId.AquaBenedictaHIT_Debuff, pad.Skill.Level, 0, pad.Trigger.RemainingLifeTime, args.Creator, pad.Skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.AquaBenedictaHIT_Debuff);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			PadDamageEnemy(args.Trigger);
		}
	}
}
