using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Monk
{
	/// <summary>
	/// Handler for the ArmorBreak debuff applied by Hand Knife.
	/// Reduces physical defense by 2% per ability level of Monk5.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ArmorBreak)]
	public class ArmorBreakOverride : BuffHandler
	{
		private const float DefRatePerLevel = -0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var penalty = DefRatePerLevel * buff.NumArg1;

			AddPropertyModifier(buff, target, PropertyName.DEF_RATE_BM, penalty);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM);
		}
	}
}
