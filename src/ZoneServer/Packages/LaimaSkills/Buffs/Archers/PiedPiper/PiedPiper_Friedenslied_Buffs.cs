using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Archers.PiedPiper
{
	/// <summary>
	/// Handler for Friedenslied on allies, who dance and take no damage.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Friedenslied_Buff)]
	public class PiedPiper_Friedenslied_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Dancing);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Dancing);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Friedenslied_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target.IsBuffActive(BuffId.Friedenslied_Buff))
				skillHitResult.Damage = 0;
		}
	}

	/// <summary>
	/// Handler for Friedenslied on enemies, who dance and lose one buff.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Friedenslied_Debuff, BuffId.Friedenslied_AbilDance_Debuff)]
	public class PiedPiper_Friedenslied_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Dancing);
			buff.Target.Components.Get<BuffComponent>()?.RemoveRandomBuff();
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Dancing);
		}
	}
}
