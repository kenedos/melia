using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Barrier: Religious Vigor, Magic Damage Increase.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Barrier_MagicAttack_Buff)]
	public class Barrier_MagicAttack_BuffOverride : BuffHandler
	{
		private const float Rate = 0.10f;
		private const string ModPropertyName = PropertyName.MATK_BM;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var modValue = Rate * buff.NumArg1;

			AddPropertyModifier(buff, target, ModPropertyName, modValue);
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
