using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Scouts.Squire
{
	/// <summary>
	/// Handler for the Base Camp buff, which raises the experience the
	/// character earns while it lasts.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.BaseCamp_Buff)]
	public class Squire_BaseCamp_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = GetCaptionRatio(buff, 1);

			AddPropertyModifier(buff, buff.Target, PropertyName.BonusExp_BM, bonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.BonusJobExp_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BonusExp_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.BonusJobExp_BM);
		}
	}
}
