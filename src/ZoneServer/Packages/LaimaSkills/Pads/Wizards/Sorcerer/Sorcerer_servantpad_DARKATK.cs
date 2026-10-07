using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Wizards.Sorcerer
{
	[Package("laima-skills")]
	[PadHandler(PadName.servantpad_DARKATK)]
	public class Sorcerer_servantpad_DARKATKOverride : ICreatePadHandler, IEnterPadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			pad.SetRange(100f);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(30);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			PadTargetBuff(pad, initiator, RelationType.Party, 0, 0, BuffId.ServantDARKATK_Buff, pad.Skill.Level, 0, 1800000, 1, 100, false);
		}
	}
}
