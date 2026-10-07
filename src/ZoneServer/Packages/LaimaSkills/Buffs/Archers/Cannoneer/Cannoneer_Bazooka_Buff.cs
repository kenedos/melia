using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for Bazooka, which raises the final damage of Cannon Shot
	/// and Cannon Barrage by the skill's ratio in percent.
	/// </summary>
	/// <remarks>
	/// Their range, AoE Attack Ratio and cooldown under Bazooka are in
	/// CannoneerSkillHelper and the skills' property overrides.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Bazooka_Buff)]
	public class Cannoneer_Bazooka_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target.Components.TryGet<SkillComponent>(out var skillComponent))
				skillComponent.InvalidateAll();
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target.Components.TryGet<SkillComponent>(out var skillComponent))
				skillComponent.InvalidateAll();
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Bazooka_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id != SkillId.Cannoneer_CannonShot && skill.Id != SkillId.Cannoneer_CannonBarrage)
				return;

			if (!attacker.TryGetBuff(BuffId.Bazooka_Buff, out var buff))
				return;

			modifier.FinalDamageMultiplier += GetCaptionRatio(buff, 1) / 100f;
		}
	}
}
