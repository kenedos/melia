using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for Protection Magic, which blocks one knockback or
	/// knockdown on a Necromancer's summon and then ends.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.SkullFollowPainBarrier_Buff)]
	public class Necromancer_SkullFollowPainBarrier_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			target.StopBuff(buff.Id);
			return KnockResult.Prevent;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			target.StopBuff(buff.Id);
			return KnockResult.Prevent;
		}
	}
}
