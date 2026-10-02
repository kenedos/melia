using Melia.Zone.Buffs;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		/// <summary>
		/// Ends every disguise on the character, whatever put it there.
		/// </summary>
		/// <remarks>
		/// The client asks for this when the player cancels a transformation,
		/// which says nothing about what caused it, so the buffs that hold a
		/// disguise are found by what they are rather than by name. Each one
		/// puts the character back the way it found it.
		/// </remarks>
		public void StopTransformation()
		{
			var buffs = this.Components.Get<BuffComponent>();

			if (buffs == null)
				return;

			foreach (var buff in buffs.GetAll(b => b.Handler is ITransformationBuff))
				this.StopBuff(buff.Id);
		}
	}
}