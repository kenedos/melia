using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Items
{
	/// <summary>
	/// Goddess' Retribution Potion buffs. Each variant grants the
	/// attacker a final-damage multiplier against boss monsters of a
	/// specific race for the buff's duration. The multiplier comes from
	/// the buff's NumArg1, expressed as a percentage (100 = +100%).
	/// </summary>
	internal static class GoddessRetributionHelper
	{
		/// <summary>
		/// Applies the +NumArg1% final-damage bonus when the target is a
		/// boss monster matching the given race.
		/// </summary>
		///
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skillHitResult"></param>
		/// <param name="buffId"></param>
		/// <param name="targetRace"></param>
		public static void ApplyAgainstBossOfRace(ICombatEntity attacker, ICombatEntity target, SkillHitResult skillHitResult, BuffId buffId, RaceType targetRace)
		{
			if (!attacker.TryGetBuff(buffId, out var buff))
				return;

			if (skillHitResult.Damage <= 0)
				return;

			if (target is not Mob mob)
				return;

			if (mob.Rank != MonsterRank.Boss)
				return;

			if (mob.Race != targetRace)
				return;

			var multiplier = 1f + (buff.NumArg1 / 100f);
			skillHitResult.Damage *= multiplier;
		}
	}

	/// <summary>
	/// Devil-race variant. +NumArg1% final damage against Velnias-race bosses.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Potion_Demon_DMG_UP_Buff)]
	public class Potion_Demon_DMG_UP_Buff : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Potion_Demon_DMG_UP_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			GoddessRetributionHelper.ApplyAgainstBossOfRace(attacker, target, skillHitResult, BuffId.Potion_Demon_DMG_UP_Buff, RaceType.Velnias);
		}
	}

	/// <summary>
	/// Mutant-race variant. +NumArg1% final damage against Paramune-race bosses.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Potion_MIX_DMG_UP_Buff)]
	public class Potion_MIX_DMG_UP_Buff : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Potion_MIX_DMG_UP_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			GoddessRetributionHelper.ApplyAgainstBossOfRace(attacker, target, skillHitResult, BuffId.Potion_MIX_DMG_UP_Buff, RaceType.Paramune);
		}
	}

	/// <summary>
	/// Insect-race variant. +NumArg1% final damage against Klaida-race bosses.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Potion_Bug_DMG_UP_Buff)]
	public class Potion_Bug_DMG_UP_Buff : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Potion_Bug_DMG_UP_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			GoddessRetributionHelper.ApplyAgainstBossOfRace(attacker, target, skillHitResult, BuffId.Potion_Bug_DMG_UP_Buff, RaceType.Klaida);
		}
	}

	/// <summary>
	/// Plant-race variant. +NumArg1% final damage against Forester-race bosses.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Potion_Plant_DMG_UP_Buff)]
	public class Potion_Plant_DMG_UP_Buff : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Potion_Plant_DMG_UP_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			GoddessRetributionHelper.ApplyAgainstBossOfRace(attacker, target, skillHitResult, BuffId.Potion_Plant_DMG_UP_Buff, RaceType.Forester);
		}
	}

	/// <summary>
	/// Beast-race variant. +NumArg1% final damage against Widling-race bosses.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Potion_Wild_DMG_UP_Buff)]
	public class Potion_Wild_DMG_UP_Buff : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Potion_Wild_DMG_UP_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			GoddessRetributionHelper.ApplyAgainstBossOfRace(attacker, target, skillHitResult, BuffId.Potion_Wild_DMG_UP_Buff, RaceType.Widling);
		}
	}
}
