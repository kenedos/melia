using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Sticky Foam, Decreased movement speed by 50%.
	/// </summary>
	[BuffHandler(BuffId.Sticky_Bubble)]
	public class Sticky_Bubble : BuffHandler
	{
		private const float Rate = 0.15f;
		private const string BasePropertyName = PropertyName.MSPD;
		private const string ModPropertyName = PropertyName.MSPD_BM;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modValue = target.Properties.GetFloat(BasePropertyName) * (buff.OverbuffCounter * Rate);

			if (target.Properties.GetFloat(PropertyName.Runnable) == 1)
				target.Properties.Modify(PropertyName.Runnable, -1);

			AddPropertyModifier(buff, buff.Target, ModPropertyName, -modValue);
		}

		/// <summary>
		/// Ends the buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			if (target.Properties.GetFloat(PropertyName.Runnable) == 0)
				target.Properties.Modify(PropertyName.Runnable, 1);

			RemovePropertyModifier(buff, buff.Target, ModPropertyName);
		}
	}
}
