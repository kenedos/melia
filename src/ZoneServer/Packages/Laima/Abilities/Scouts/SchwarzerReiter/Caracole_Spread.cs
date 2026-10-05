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
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Ability handler for Caracole: Spread.
	/// Makes Caracole damage spread to nearby enemies around each hit target.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Schwarzereiter17)]
	public class SchwarzerReiter_Caracole_SpreadAbility : IAbilityHandler
	{
		private const int MaxSpreadTargets = 5;
		private const int SpreadRadius = 120;

		/// <summary>
		/// Applies additional spread damage after Caracole damage is calculated.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Schwarzereiter17)]
		public void OnAttackAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			// This ability should only affect Caracole.
			if (skill.Id != SkillId.Schwarzereiter_Caracole)
				return;

			// Only characters can own abilities.
			if (attacker is not Character character)
				return;

			// Verify that the ability is learned.
			var abilityLevel = character.Abilities.GetLevel(AbilityId.Schwarzereiter17);

			if (abilityLevel <= 0)
				return;

			// Ignore invalid targets or maps.
			if (target == null || target.Map == null)
				return;

			// Create a circular area around the main target.
			var spreadArea = new Circle(target.Position, SpreadRadius);

			// Find nearby enemies, excluding the original target.
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
				// Apply a reduced copy of the original Caracole damage.
				// Adjust this ratio if the original ability uses a different value.
				var spreadDamage = skillHitResult.Damage * 0.5f;

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
