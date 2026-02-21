using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for the Priest Revive buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Cleric_Revival_Buff)]
	public class Cleric_Revival_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Activates only when hp is 1
			// When target would be killed, we set their hp to 1
			// and call this activate method again.
			if (buff.Target.Hp > 1)
				return;

			var target = buff.Target;
			var healAmount = buff.NumArg2;

			target.Heal(healAmount, 0);

			Send.ZC_GROUND_EFFECT(target, target.Position, "F_cleric_Revival_light", 0.5f);

			target.RemoveBuff(buff.Id);
		}
	}
}
