using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;

/// <summary>
/// Handle for the Frozen, Frozen solid..
/// </summary>
[BuffHandler(BuffId.Sleep_Debuff, BuffId.UC_sleep)]
public class Sleep : BuffHandler
{
	public override void OnActivate(Buff buff, ActivationType activationType)
	{
		buff.Target.AddState(StateType.Sleep);
	}

	[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Sleep_Debuff)]
	[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.UC_sleep)]
	public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		target.StopBuff(BuffId.Sleep_Debuff);
		target.StopBuff(BuffId.UC_sleep);
	}

	public override void OnEnd(Buff buff)
	{
		buff.Target.RemoveState(StateType.Sleep);
	}
}
