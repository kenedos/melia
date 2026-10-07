using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Long-ranged Shot buff, which extends the range of
	/// the basic attack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Schwarzereiter_MaxR_Buff)]
	public class SchwarzerReiter_MaxR_BuffOverride : BuffHandler
	{
		private const float BonusRange = 100f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MaxR_BM, BonusRange);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MaxR_BM);
		}
	}
}
