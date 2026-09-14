using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsmen.Swordsman
{
	/// <summary>
	/// Handle for the Gung Ho Buff, which increases the target's attack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.GungHo)]
	public class GungHoOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = GetCaptionRatio(buff, 1) / 100f;

			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
		}
	}
}
