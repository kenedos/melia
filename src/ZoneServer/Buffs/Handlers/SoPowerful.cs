using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Increased Attack, Increased attack.
	/// </summary>
	[BuffHandler(BuffId.SoPowerful)]
	public class SoPowerful : BuffHandler
	{
		private const int Bonus = 9999999;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Properties.Modify(PropertyName.PATK_BM, Bonus);
			buff.Target.Properties.Modify(PropertyName.MATK_BM, Bonus);
			buff.Target.Properties.Modify(PropertyName.HR_BM, Bonus);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.Properties.Modify(PropertyName.PATK_BM, -Bonus);
			buff.Target.Properties.Modify(PropertyName.MATK_BM, -Bonus);
			buff.Target.Properties.Modify(PropertyName.HR_BM, -Bonus);
		}
	}
}
