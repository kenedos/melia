using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Enhance patience, Damage received decrease by 20%.
	/// </summary>
	[BuffHandler(BuffId.FortitudeEndure)]
	public class FortitudeEndure : BuffHandler
	{
		private const int Bonus = -20;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Properties.Modify(PropertyName.DMG_MTPL_BM, Bonus);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.Properties.Modify(PropertyName.DMG_MTPL_BM, -Bonus);
		}
	}
}
