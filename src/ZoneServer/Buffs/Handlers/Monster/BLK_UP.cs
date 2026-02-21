using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Increases Block chance, Increases Block chance.
	/// </summary>
	[BuffHandler(BuffId.BLK_UP)]
	public class BLK_UP : BuffHandler
	{
		private const float Rate = 0.6f;
		private const string BasePropertyName = PropertyName.BLK;
		private const string ModPropertyName = PropertyName.BLK_BM;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modValue = target.Properties.GetFloat(BasePropertyName) * Rate * buff.NumArg1;

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
