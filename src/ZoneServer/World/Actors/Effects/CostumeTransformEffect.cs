using Melia.Shared.Game.Const;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	/// <summary>
	/// A costume transform "look override" attached to an actor. Shown via
	/// <see cref="Send.ZC_NORMAL.UpdateCharacterLook"/> to each observer that
	/// comes into visibility, and reverted on removal.
	/// </summary>
	/// <remarks>
	/// Stored as a named effect on the actor's <see cref="Components.EffectsComponent"/>
	/// so Laima's visibility pipeline (<c>ShowEffects</c> in HandleAppearingCharacters)
	/// replays it automatically — no per-observer broadcast code needed.
	/// </remarks>
	public class CostumeTransformEffect : Effect
	{
		public string TransformClassName { get; }
		public EquipSlot Slot { get; }

		public CostumeTransformEffect(string transformClassName, EquipSlot slot)
		{
			this.TransformClassName = transformClassName;
			this.Slot = slot;
		}

		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.UpdateCharacterLook(conn, actor, this.TransformClassName, this.Slot, 0, 1);
		}

		public override void OnRemove(IActor actor)
		{
			// Broadcast the revert (itemId = 0) so every observer sees the
			// base costume return, mirroring the initial AddEffect broadcast.
			Send.ZC_NORMAL.UpdateCharacterLook(actor, 0, this.Slot, 0);
		}
	}
}
