using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for Ram Muay buff.
	/// Updates the character's basic attack while Ram Muay stance is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.RamMuay_Buff)]
	public class NakMuay_RamMuay_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			if (buff.Target is Character character)
				NakMuayAttackHelper.UpdateMainAttack(character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
				NakMuayAttackHelper.UpdateMainAttack(character);
		}
	}
}
