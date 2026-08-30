using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Lunge buff, which raises the target's evasion.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Lunge_Buff)]
	public class Fencer_Lunge_BuffOverride : BuffHandler
	{
		private const float EvasionBase = 50f;
		private const float EvasionPerLevel = 20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, EvasionBase + buff.NumArg1 * EvasionPerLevel);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
