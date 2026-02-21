using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	public abstract class Effect
	{
		/// <summary>
		/// Show's an effect to a specific connection
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		/// <returns></returns>
		public abstract void ShowEffect(IZoneConnection conn, IActor actor);

		/// <summary>
		/// Called when the effect is removed from its owner.
		/// </summary>
		/// <param name="actor">The actor that owned the effect.</param>
		public virtual void OnRemove(IActor actor) { }
	}
}
