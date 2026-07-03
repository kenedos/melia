using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// RuneCaster6 - Rune of Protection: Maintain Casting.
	///
	/// While Rune of Protection is active, prevents casting interruption by
	/// reducing incoming damage during Rune Caster casting skills.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.RuneCaster6)]
	public class RuneCaster_RuneOfProtectionMaintainCastingAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, AbilityId.RuneCaster6)]
		public void OnDefenseAfterCalc(
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.RuneOfProtection_Buff, out _))
				return;

			if (!this.IsCastingRuneCasterSkill(target))
				return;

			// Melia does not expose a direct "do not interrupt casting" flag here.
			// Reducing the received damage to 0 prevents damage-based interruption
			// in most cases and keeps the cast active.
			skillHitResult.Damage = 0;
		}

		private bool IsCastingRuneCasterSkill(ICombatEntity target)
		{
			return
				this.IsCasting(target, SkillId.RuneCaster_Hagalaz) ||
				this.IsCasting(target, SkillId.RuneCaster_Isa) ||
				this.IsCasting(target, SkillId.RuneCaster_Thurisaz) ||
				this.IsCasting(target, SkillId.RuneCaster_Tiwaz) ||
				this.IsCasting(target, SkillId.RuneCaster_Stan) ||
				this.IsCasting(target, SkillId.RuneCaster_Ehwaz);
		}

		private bool IsCasting(ICombatEntity target, SkillId skillId)
		{
			return target.TryGetSkill(skillId, out var runeSkill) && target.IsCasting(runeSkill);
		}
	}
}
