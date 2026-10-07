using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// Limacon: Spread ability, which carries Limacon's pistol shots to up
	/// to 3 more enemies, and Marching Fire's shots to 1 more.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Schwarzereiter18)]
	public class Schwarzereiter18Override : IAbilityHandler
	{
		private const float SpreadRadius = 50f;
		private const int LimaconSpreadTargets = 3;
		private const int MarchingFireSpreadTargets = 1;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter18)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage <= 0 || !attacker.IsAbilityActive(AbilityId.Schwarzereiter18))
				return;

			var spreadTargets = 0;
			if (skill.Id == SkillId.Pistol_Attack2 && attacker.IsBuffActive(BuffId.Limacon_Buff))
				spreadTargets = LimaconSpreadTargets;
			else if (skill.Id == SkillId.Schwarzereiter_AssaultFire)
				spreadTargets = MarchingFireSpreadTargets;

			if (spreadTargets == 0)
				return;

			var targets = attacker.Map.GetAttackableEnemiesInPosition(attacker, target.Position, SpreadRadius)
				.Where(t => t != target && !t.IsDead)
				.Take(spreadTargets);

			foreach (var spreadTarget in targets)
			{
				spreadTarget.TakeDamage(skillHitResult.Damage, attacker);
				Send.ZC_HIT_INFO(attacker, spreadTarget, new HitInfo(attacker, spreadTarget, skill, skillHitResult.Damage, HitResultType.Hit));
			}
		}
	}
}
