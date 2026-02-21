using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Decreased movement speed, Decreased movement speed..
	/// </summary>
	[BuffHandler(BuffId.Rage_Rockto_spd_down)]
	public class Rage_Rockto_spd_down : BuffHandler
	{
		private const float Rate = -0.7f;
		private const string BasePropertyName = PropertyName.MSPD;
		private const string ModPropertyName = PropertyName.MSPD_BM;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modValue = target.Properties.GetFloat(BasePropertyName) * Rate;

			AddPropertyModifier(buff, buff.Target, ModPropertyName, modValue);
		}

		/// <summary>
		/// Ends the buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, ModPropertyName);
		}
	}
}
