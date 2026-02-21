using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Contagious Deadly Poison, Decreased attack. 
	/// Periodically receiving poison damage. The effect can be transferred to nearby targets..
	/// </summary>
	[BuffHandler(BuffId.SpreadPoison)]
	public class SpreadPoison : BuffHandler
	{
		private const int DecreaseAttackConst = -3;
		private const float HpChange = 0.05f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Properties.Modify(PropertyName.ATK_BM, buff.NumArg1 * DecreaseAttackConst);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.Properties.Modify(PropertyName.ATK_BM, buff.NumArg1 * -DecreaseAttackConst);
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			var maxHp = target.Properties.GetFloat(PropertyName.MHP);

			if (target.Hp > 0)
				target.TakeDamage(maxHp * HpChange, null);

			// TODO: Decide if spread logic would be here
			// Only spreads to same faction as target
			foreach (var spreadTarget in target.Map.GetAttackableEnemiesInPosition(target, target.Position, 20))
			{
				spreadTarget.StartBuff(BuffId.SpreadPoison, buff.Duration);
			}
		}
	}
}
