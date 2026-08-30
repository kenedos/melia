using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Evasive Action buff, which raises the target's
	/// evasion.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EvasiveAction_Buff)]
	public class SchwarzerReiter_EvasiveAction_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM, GetCaptionRatio(buff, 1) / 100f);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
