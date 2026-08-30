using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Lunge debuff, which leaves the target more vulnerable
	/// to Slash attacks.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Lunge_Debuff)]
	public class Fencer_Lunge_DebuffOverride : BuffHandler
	{
		private const float SlashDamageBonus = 0.10f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Lunge_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.Lunge_Debuff))
				return;

			if (skill.Data.AttackType != SkillAttackType.Slash)
				return;

			skillHitResult.Damage *= 1f + SlashDamageBonus;
		}
	}
}
