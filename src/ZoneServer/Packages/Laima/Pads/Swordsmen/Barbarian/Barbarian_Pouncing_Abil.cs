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

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Barbarian_Pouncing_Abil)]
	public class Barbarian_Pouncing_AbilOverride : ICreatePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			// Note: The skill handler creates the Square area with proper dimensions
			// We don't override it here
			pad.SetUpdateInterval(200);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(3500);
			pad.Trigger.MaxActorCount = 5;
			creator.PlaySound("skl_eff_barbarian_pouncing_whoosh_abil");
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (!creator.IsBuffActive(BuffId.Pouncing_Buff))
			{
				pad.Destroy();
				return;
			}

			// Update the pad's area shape to match new position/direction
			pad.Movement.MoveTo(creator.Position);
			pad.Direction = creator.Direction;

			// Recreate Square area with updated direction (Square.Direction is readonly)
			pad.SetRectangleRange(creator.Direction, 35f, 90f);

			// Deal damage using PadDamageEnemy
			PadDamageEnemy(pad, 1f, 0, 0, "F_hit_slash", 3, 0f, 0f);
		}
	}
}
