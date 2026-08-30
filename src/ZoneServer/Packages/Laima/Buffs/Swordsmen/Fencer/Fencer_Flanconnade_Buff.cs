using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Flanconnade buff, which doubles the target's block
	/// while the skill is in use.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Flanconnade_Buff)]
	public class Fencer_Flanconnade_BuffOverride : BuffHandler
	{
		private const float BlockRate = 1f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_RATE_BM, BlockRate);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_RATE_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
