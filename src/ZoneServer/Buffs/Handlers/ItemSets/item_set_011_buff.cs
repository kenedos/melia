using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.ItemSets
{
	/// <summary>
	/// Handler for the Watcher/Sentinel Set buff (item_set_011_buff).
	/// Adds +157 Velnias Attack when active.
	/// </summary>
	[BuffHandler(BuffId.item_set_011_buff)]
	public class item_set_011_buff : BuffHandler
	{
		private const float VelniasAtkBonus = 157f;

		/// <summary>
		/// Applies the Velnias attack bonus when the buff starts.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			AddPropertyModifier(buff, target, PropertyName.Velnias_Atk_BM, VelniasAtkBonus);

			if (target is Character character)
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Velnias_Atk_BM);
		}

		/// <summary>
		/// Removes the Velnias attack bonus when the buff ends.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.Velnias_Atk_BM);

			if (target is Character character)
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Velnias_Atk_BM);
		}
	}
}
