using System.Collections.Generic;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Handlers.Base
{
	public interface ISkillHandler
	{
	}

	public interface ITargetSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster, ICombatEntity target);
	}

	public interface IMeleeGroundSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets);
	}

	public interface ITargetGroundSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target);
	}

	public interface IForceGroundSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target);
	}

	public interface IForceSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target);
	}

	public interface ISelfSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir);
	}

	public interface IPassiveSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster);
	}

	/// <summary>
	/// A skill handler interface when the client provides a target
	/// </summary>
	public interface IDynamicCasted : ISkillHandler
	{
		void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime);
		void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime);
	}

	public interface ICancelSkillHandler : ISkillHandler
	{
		void Handle(Skill skill, ICombatEntity caster);
	}

	public interface ISkillCombatAttackBeforeCalcHandler { void OnAttackBeforeCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatDefenseBeforeCalcHandler { void OnDefenseBeforeCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface ISkillCombatAttackAfterCalcHandler { void OnAttackAfterCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatDefenseAfterCalcHandler { void OnDefenseAfterCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface ISkillCombatAttackBeforeBonusesHandler { void OnAttackBeforeBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatDefenseBeforeBonusesHandler { void OnDefenseBeforeBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface ISkillCombatAttackAfterBonusesHandler { void OnAttackAfterBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatDefenseAfterBonusesHandler { void OnDefenseAfterBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	// Companion-specific combat hook interfaces - only called when a companion attacks and the owner has this passive skill
	public interface ISkillCombatCompanionAttackBeforeCalcHandler { void OnCompanionAttackBeforeCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatCompanionDefenseBeforeCalcHandler { void OnCompanionDefenseBeforeCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface ISkillCombatCompanionAttackAfterCalcHandler { void OnCompanionAttackAfterCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatCompanionDefenseAfterCalcHandler { void OnCompanionDefenseAfterCalc(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface ISkillCombatCompanionAttackBeforeBonusesHandler { void OnCompanionAttackBeforeBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatCompanionDefenseBeforeBonusesHandler { void OnCompanionDefenseBeforeBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	public interface ISkillCombatCompanionAttackAfterBonusesHandler { void OnCompanionAttackAfterBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }
	public interface ISkillCombatCompanionDefenseAfterBonusesHandler { void OnCompanionDefenseAfterBonuses(Skill skill, ICombatEntity attacker, ICombatEntity target, Skill attackerSkill, SkillModifier modifier, SkillHitResult skillHitResult); }

	// Buff event hook interfaces - called when buffs are applied to the character
	public interface ISkillOnBuffStartHandler { void OnBuffStart(Skill skill, ICombatEntity target, Buff buff); }
	public interface ISkillOnBuffEndHandler { void OnBuffEnd(Skill skill, ICombatEntity target, Buff buff); }
}
