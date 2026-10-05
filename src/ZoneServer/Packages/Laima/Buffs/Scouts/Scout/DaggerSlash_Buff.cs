using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scout
{
	/// <summary>
	/// Handler for the Dagger Slash Buff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DaggerSlash_Buff)]
	public class DaggerSlash_BuffOverride : BuffHandler
	{
		private const float BuffBonus = 4f;

		/// <summary>
		/// Starts buff, modifying the target's movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.OverbuffCounter <= 3)
			{
				AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, BuffBonus);

				buff.Target.TryGetProp(PropertyName.MSPD, out float mspd);
			}
		}

		/// <summary>
		/// Ends the buff, resetting the movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
