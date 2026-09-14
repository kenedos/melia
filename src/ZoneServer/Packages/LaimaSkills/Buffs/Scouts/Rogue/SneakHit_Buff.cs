using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Scouts.Rogue
{
	/// <summary>
	/// Handler for the SneakHit buff. Increases back attack damage.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.SneakHit_Buff)]
	public class SneakHit_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.SneakHit_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.SneakHit_Buff, out var buff))
				return;

			if (!attacker.IsBehind(target) && !modifier.ForcedBackAttack)
				return;

			var damageIncrease = GetCaptionRatio(buff, 1) / 100f;

			skillHitResult.Damage *= 1f + damageIncrease;
		}
	}
}
