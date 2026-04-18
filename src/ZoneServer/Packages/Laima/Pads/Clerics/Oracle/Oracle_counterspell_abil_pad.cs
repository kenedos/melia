using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Oracle
{
	[Package("laima")]
	[PadHandler(PadName.counterspell_abil_pad)]
	public class Oracle_counterspell_abil_padOverride : ICreatePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var skill = pad.Skill;

			pad.SetRange(70f);
			pad.SetUpdateInterval(1000);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000 + skill.Level * 1000);
			pad.Trigger.MaxActorCount = 5 + skill.Level;
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			PadDamageEnemy(pad, 1f, 0, 0, "None", 1, 0f, 0f);
		}
	}
}
