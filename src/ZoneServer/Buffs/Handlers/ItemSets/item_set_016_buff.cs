using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.ItemSets
{
	/// <summary>
	/// Handler for the Legwyn Family Set buff (item_set_016_buff).
	/// Heals 40 SP on activation and every update tick.
	/// </summary>
	[BuffHandler(BuffId.item_set_016_buff)]
	public class item_set_016_buff : BuffHandler
	{
		private const int SpHealAmount = 40;

		/// <summary>
		/// Heals SP when the buff starts.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character character)
			{
				character.ModifySp(SpHealAmount);
			}
		}

		/// <summary>
		/// Heals SP on each update tick.
		/// </summary>
		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
			{
				character.ModifySp(SpHealAmount);
			}
		}
	}
}
