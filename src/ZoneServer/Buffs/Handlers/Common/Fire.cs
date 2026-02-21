using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Buff handler for Fire, which deals fire damage in regular intervals.
	/// </summary>
	/// <remarks>
	/// NumArg2: Snapshotted damage per tick (pre-calculated by skill handler)
	/// </remarks>
	[BuffHandler(BuffId.Fire, BuffId.UC_flame, BuffId.Mon_FireWall)]
	public class Fire : DamageOverTimeBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Call base to snapshot damage from NumArg2
			base.OnActivate(buff, activationType);

			// Show flame emoticon
			var target = buff.Target;
			Send.ZC_SHOW_EMOTICON(target, "I_emo_flame", buff.Duration);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Fire;
		}
	}
}
