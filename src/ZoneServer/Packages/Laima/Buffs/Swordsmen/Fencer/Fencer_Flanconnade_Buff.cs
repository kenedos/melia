using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Flanconnade buff, which raises the target's evasion
	/// while the skill is in use.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Flanconnade_Buff)]
	public class Fencer_Flanconnade_BuffOverride : BuffHandler
	{
		private const float EvasionRate = 0.5f;

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
