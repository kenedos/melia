using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Pads.HandlersOverride.Archers.Fletcher
{
	[Package("laima")]
	[PadHandler(PadName.Fletcher_CatenaChainArrow_PAD)]
	public class Fletcher_CatenaChainArrowOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float MaxLeashDistance = 150f;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			if (pad.Variables.TryGet<Npc>("Melia.Pad.CatenaAnchor", out var anchor))
				anchor.DisappearTime = DateTime.Now;

			creator.RemoveEffect("Melia.Skill.CatenaChainLink");
			creator.StopBuff(BuffId.Fletcher_CatenaChainArrow_Buff);

			var skill = pad.Skill;
			if (skill != null)
				skill.Vars.SetInt("Melia.Skill.CatenaPadHandle", 0);

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			var distance = creator.Position.Get2DDistance(pad.Position);
			if (distance >= MaxLeashDistance)
			{
				Send.ZC_NORMAL.PadUpdate(creator, pad, false);
				creator.Map.RemovePad(pad);
			}
		}
	}
}
