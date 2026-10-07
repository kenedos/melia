using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for the Smoke Grenade pad, whose smoke blinds the enemies in
	/// it and reveals the hidden ones.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Cannoneer_SmokeGrenade)]
	public class Cannoneer_SmokeGrenadeOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler
	{
		private const float Range = 75f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.Trigger.LifeTime = pad.Skill.Properties.CaptionTime;
			pad.Trigger.MaxActorCount = (int)pad.Skill.Properties.GetFloat(PropertyName.CaptionRatio);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.SmokeGrenade_Debuff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var target = args.Initiator;

			if (!args.Creator.IsEnemy(target))
				return;

			target.StopBuffByTag(BuffTag.Cloaking);
			target.StartBuff(BuffId.SmokeGrenade_Debuff, pad.Skill.Level, 0, pad.Trigger.RemainingLifeTime, args.Creator, pad.Skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			args.Initiator.StopBuff(BuffId.SmokeGrenade_Debuff);
		}
	}
}
