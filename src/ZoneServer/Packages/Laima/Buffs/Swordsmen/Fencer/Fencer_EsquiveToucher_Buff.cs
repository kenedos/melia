using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Esquive Toucher buff, which doubles the target's
	/// evasion while the skill is in use.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EsquiveToucher_Buff)]
	public class Fencer_EsquiveToucher_BuffOverride : BuffHandler
	{
		private const float EvasionRate = 1f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM, EvasionRate);
			buff.Target.InvalidateProperties();
		}

		public override void WhileActive(Buff buff)
		{
			Fencer_RapierGuard.EndWithoutRapier(buff);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
