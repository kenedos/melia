using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Rapier requirement shared by the Fencer buffs, none of which survive
	/// the weapon they were granted with being swapped out.
	/// </summary>
	public static class Fencer_RapierGuard
	{
		/// <summary>
		/// Returns true if the entity is wielding a rapier in its main hand.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool HasRapier(ICombatEntity entity)
			=> entity.TryGetEquipItem(EquipSlot.RightHand, out var equipItem) && equipItem.Data.EquipType1 == EquipType.Rapier;

		/// <summary>
		/// Ends the buff if its target is no longer wielding a rapier,
		/// returning whether it was ended.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		public static bool EndWithoutRapier(Buff buff)
		{
			if (HasRapier(buff.Target))
				return false;

			buff.Target.StopBuff(buff.Id);
			return true;
		}
	}
}
