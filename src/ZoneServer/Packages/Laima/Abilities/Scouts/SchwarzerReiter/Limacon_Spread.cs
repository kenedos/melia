using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Limacon: Spread.
	/// Makes Limacon attacks spread damage to nearby enemies around the main target.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter18)]
	public class SchwarzerReiter_Limacon_SpreadAbility : IAbilityHandler
	{
		private const int MaxSpreadTargets = 5;
		private const int SpreadRadius = 120;

		/// <summary>
		/// Applies spread damage after the main Limacon hit is calculated.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter18)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// Limacon spread only works while Limacon is active.
			if (!attacker.TryGetBuff(BuffId.Limacon_Buff, out _))
				return;

			// Limacon uses Pistol_Attack2 as its main attack.
			if (skill.Id != SkillId.Pistol_Attack2)
				return;

			// Ignore invalid targets or maps.
			if (target == null || target.Map == null)
				return;

			// Create a circular spread area around the main target.
			var spreadArea = new Circle(target.Position, SpreadRadius);

			// Find up to 5 additional enemies around the main target.
			var spreadTargets = target.Map
				.GetAttackableEnemiesIn(attacker, spreadArea)
				.Where(enemy => enemy != null)
				.Where(enemy => !enemy.IsDead)
				.Where(enemy => enemy.Handle != target.Handle)
				.OrderBy(enemy => enemy.Position.Get2DDistance(target.Position))
				.Take(MaxSpreadTargets)
				.ToList();

			foreach (var spreadTarget in spreadTargets)
			{
				// Apply the same final damage from the main Limacon hit.
				var spreadDamage = skillHitResult.Damage;

				spreadTarget.TakeDamage(spreadDamage, attacker);

				// Send visual hit information to the client.
				var hitInfo = new HitInfo(
					attacker,
					spreadTarget,
					skill,
					spreadDamage,
					skillHitResult.Result);

				Send.ZC_HIT_INFO(attacker, spreadTarget, hitInfo);
			}
		}
	}
}
