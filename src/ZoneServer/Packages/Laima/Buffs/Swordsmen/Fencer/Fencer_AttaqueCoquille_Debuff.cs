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
	/// Handler for the Attaque Coquille debuff, which lets Pierce attacks
	/// ignore part of the target's defense.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.AttaqueCoquille_Debuff)]
	public class Fencer_AttaqueCoquille_DebuffOverride : BuffHandler
	{
		private const float DefensePenetration = 0.20f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.AttaqueCoquille_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.IsBuffActive(BuffId.AttaqueCoquille_Debuff))
				return;

			if (skill.Data.AttackType != SkillAttackType.Aries)
				return;

			modifier.DefensePenetrationRate += DefensePenetration;
		}
	}
}
