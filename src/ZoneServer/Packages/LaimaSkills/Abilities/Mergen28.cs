using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers
{
	/// <summary>
	/// [Arts] Mergen: Continue Hunting ability, which restores 20% of the
	/// Mergen's HP recovery when a Mergen attack kills a small or medium
	/// monster, at most once every 5 seconds.
	/// </summary>
	[Package("laima-skills")]
	[AbilityHandler(AbilityId.Mergen28)]
	public class Mergen28Override : IAbilityHandler
	{
		private const float RecoveryRate = 0.20f;
		private const string LastRecoveryVar = "Melia.Mergen.ContinueHunting";
		private static readonly TimeSpan Cooldown = TimeSpan.FromSeconds(5);

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.Mergen28)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || !character.IsAbilityActive(AbilityId.Mergen28))
				return;

			if (!MergenSkillHelper.IsAttackSkill(skill) || skillHitResult.Damage < target.Hp)
				return;

			if (target.EffectiveSize != SizeType.S && target.EffectiveSize != SizeType.M)
				return;

			var now = GameClock.LocalNow;
			if (now - character.Variables.Temp.Get<DateTime>(LastRecoveryVar, DateTime.MinValue) < Cooldown)
				return;

			character.Variables.Temp.Set(LastRecoveryVar, now);
			character.Heal(character.Properties.GetFloat(PropertyName.RHP) * RecoveryRate, 0);
		}
	}
}
