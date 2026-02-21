using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Slow, Decreased movement speed.
	/// </summary>
	[BuffHandler(BuffId.Monster_Slow)]
	public class Monster_Slow : BuffHandler
	{
		private const float Rate = 0.2f;
		private const string BasePropertyName = PropertyName.MSPD;
		private const string ModPropertyName = PropertyName.MSPD_BM;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modValue = target.Properties.GetFloat(BasePropertyName) * (0.85f + buff.NumArg1 * Rate);

			AddPropertyModifier(buff, buff.Target, ModPropertyName, -modValue);
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
