using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Twist of Fate debuff, which periodically heals
	/// the target based on accumulated damage rate.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.TwistOfFate_Debuff)]
	public class Oracle_TwistOfFate_DebuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			if (buff.Target.IsDead)
				return;

			var healValue = buff.Target.GetTempVar("TwistOfFate_DamageRate");
			if (healValue > 0)
				buff.Target.Heal(healValue / 30f, 0);
		}
	}
}
