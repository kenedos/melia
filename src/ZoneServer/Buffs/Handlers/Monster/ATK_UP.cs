using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Increases Attack, very nasty.
	/// </summary>
	[BuffHandler(BuffId.ATKUP)]
	public class ATK_UP : BuffHandler
	{
		private const float Rate = 0.6f;
		private const string BasePropertyName = PropertyName.PATK;
		private const string ModPropertyName = PropertyName.PATK_BM;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modValue = target.Properties.GetFloat(PropertyName.MINPATK) * Rate * buff.NumArg1;

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
