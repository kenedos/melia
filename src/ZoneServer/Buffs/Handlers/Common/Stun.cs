using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handle for the Stun Debuff. Unable to act. Increases minimum
	/// critical chance by +30% with incoming Pierce attacks.
	/// </summary>
	[BuffHandler(BuffId.Stun, BuffId.UC_stun)]
	public class Stun : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Stunned);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Stunned);
		}

		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target.IsStaggered())
				skillHitResult.Damage *= 1.5f;
		}
	}
}
