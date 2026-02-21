using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Decreased Attack, Decreased attack..
	/// </summary>
	[BuffHandler(BuffId.Rage_Rockto_atk_down)]
	public class Rage_Rockto_atk_down : BuffHandler
	{
		private const float Rate = -0.5f;
		private const string BasePropertyName = PropertyName.MINMATK;
		private const string ModPropertyName = PropertyName.MATK_BM;

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
