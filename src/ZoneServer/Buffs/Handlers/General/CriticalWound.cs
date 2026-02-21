using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandler(BuffId.CriticalWound)]
	public class CriticalWound : DamageOverTimeBuffHandler
	{
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Normal;
		}
	}
}
