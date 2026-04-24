using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Generic base class for buffs that swap a character's costume appearance
	/// to the "_af" transform variant (see <see cref="CostumeTransformDb"/>).
	/// Subclasses only need the <see cref="BuffHandlerAttribute"/>.
	/// </summary>
	/// <remarks>
	/// The transform is installed as a named <see cref="CostumeTransformEffect"/>
	/// on the character's EffectsComponent. That path:
	///  1. Broadcasts the look change to every observer currently in range.
	///  2. Replays automatically to any observer that later enters visibility
	///     (via ShowEffects in HandleAppearingCharacters).
	///  3. Calls OnRemove on buff end to broadcast the revert.
	/// </remarks>
	public abstract class CostumeTransformBuffHandler : BuffHandler
	{
		private static string EffectName(CostumeTransformData xform)
			=> "Melia.CostumeTransform." + xform.Slot;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			if (!TryResolveTransform(character, buff, out var xform))
				return;

			character.AddEffect(EffectName(xform), new CostumeTransformEffect(xform.TransformClassName, xform.Slot));
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			if (!TryResolveTransform(character, buff, out var xform))
				return;

			character.RemoveEffect(EffectName(xform));
		}

		/// <summary>
		/// Resolves the transform data for the buff. Many base costumes share a
		/// single buff name (e.g. every TOSummer male maps to ITEM_BUFF_TCANCE_BOY),
		/// so TryFindByBuff alone would always return the first match and drive
		/// every variant to the same appearance. Prefer the currently-equipped
		/// Outer costume's mapping and fall back to the by-buff lookup only when
		/// the equipped item isn't a transform costume.
		/// </summary>
		private static bool TryResolveTransform(Character character, Buff buff, out CostumeTransformData xform)
		{
			var equipped = character.Inventory.GetEquip(EquipSlot.Outer1);
			if (equipped?.Data != null
				&& CostumeTransformDb.TryFindByBase(equipped.Data.ClassName, out xform)
				&& string.Equals(xform.BuffName, buff.Data.ClassName, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}

			return CostumeTransformDb.TryFindByBuff(buff.Data.ClassName, out xform);
		}
	}
}
