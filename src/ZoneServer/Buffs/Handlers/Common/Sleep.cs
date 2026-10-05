using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;

/// <summary>
/// Handle for the Frozen, Frozen solid..
/// </summary>
[BuffHandler(BuffId.Sleep_Debuff, BuffId.UC_sleep)]
public class Sleep : BuffHandler, IBuffCombatDefenseAfterCalcHandler
{
	public override void OnActivate(Buff buff, ActivationType activationType)
	{
		buff.Target.AddState(StateType.Sleep);
	}

	public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (skillHitResult.Damage > 0)
			buff.Target.RemoveBuff(buff.Id);
	}

	public override void OnEnd(Buff buff)
	{
		buff.Target.RemoveState(StateType.Sleep);
	}
}
