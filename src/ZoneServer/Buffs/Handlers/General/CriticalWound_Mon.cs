using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandler(BuffId.CriticalWound_Mon)]
	public class CriticalWound_Mon : DamageOverTimeBuffHandler
	{
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Normal;
		}
	}
}
