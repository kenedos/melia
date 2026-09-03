using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Twist of Fate debuff, which gives the target back
	/// a tenth of the health the skill took on every tick, until all of
	/// it has been returned.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: Health returned per tick
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.TwistOfFate_Debuff)]
	public class Oracle_TwistOfFate_DebuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead)
				return;

			var healAmount = buff.NumArg2;
			if (healAmount <= 0)
				return;

			target.Heal(healAmount, 0);

			Send.ZC_HEAL_INFO(target, healAmount, target.Properties.GetFloat(PropertyName.HP), HealType.Hp);
		}
	}
}
