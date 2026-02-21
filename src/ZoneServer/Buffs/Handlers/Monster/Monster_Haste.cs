using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Haste, Increased Movement Speed.
	/// </summary>
	[BuffHandler(BuffId.Monster_Haste)]
	public class Monster_Haste : BuffHandler
	{
		private const float Rate = 2f;
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
