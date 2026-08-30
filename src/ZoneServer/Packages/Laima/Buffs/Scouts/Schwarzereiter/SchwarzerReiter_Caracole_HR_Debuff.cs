using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Caracole accuracy debuff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Caracole_HR_Debuff)]
	public class SchwarzerReiter_Caracole_HR_DebuffOverride : BuffHandler
	{
		private const float AccuracyReduction = 0.25f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM, -AccuracyReduction);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_RATE_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
