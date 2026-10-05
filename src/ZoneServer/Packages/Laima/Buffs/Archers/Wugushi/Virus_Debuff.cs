using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Virus Debuff, which ticks poison damage while active,
	/// spreads on death, and has a 20% chance to spread to one nearby enemy
	/// when the infected target is struck.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: Snapshotted damage per tick (calculated on buff application)
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Virus_Debuff)]
	public class Virus_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const int MaxSpreadOnDeathAmount = 2;
		private const float SpreadRange = 50f;
		private const float SpreadOnHitChance = 20f;
		private const int SpreadOnHitCount = 1;
		private const string RemainingDurationVar = "Melia.Virus_Debuff.RemainingDuration";

		public override void WhileActive(Buff buff)
		{
			buff.Vars.Set(RemainingDurationVar, buff.RemainingDuration);
			base.WhileActive(buff);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target.IsDead)
				this.SpreadVirusOnDeath(buff);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}

		/// <summary>
		/// When the poisoned target is struck, 20% chance to spread
		/// poison to one nearby enemy with remaining duration.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Virus_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Virus_Debuff, out var buff))
				return;

			if (buff.Caster is not ICombatEntity caster)
				return;

			if (target.IsDead)
				return;

			if (RandomProvider.Get().Next(100) >= SpreadOnHitChance)
				return;

			var map = target.Map;
			if (map == null)
				return;

			var nearbyEnemies = map.GetAttackableEnemiesIn(caster, new Circle(target.Position, SpreadRange))
				.Where(e => e != target && !e.IsBuffActive(BuffId.Virus_Debuff))
				.Take(SpreadOnHitCount);

			var remainingDuration = buff.RemainingDuration;
			var damage = buff.NumArg2;

			foreach (var enemy in nearbyEnemies)
			{
				enemy.StartBuff(BuffId.Virus_Debuff, buff.NumArg1, damage, remainingDuration, caster, buff.SkillId);
			}
		}

		private void SpreadVirusOnDeath(Buff buff)
		{
			if (buff.Caster is not ICombatEntity caster)
				return;

			var target = buff.Target;

			if (target.Map == null)
				return;

			var targetsInRange = target.Map.GetAttackableEnemiesInPosition(caster, target.Position, SpreadRange);
			var spreadTargets = targetsInRange
				.Where(a => a != target && !a.IsBuffActive(BuffId.Virus_Debuff))
				.Take(MaxSpreadOnDeathAmount);

			var damage = buff.NumArg2;

			if (!buff.Vars.TryGet<TimeSpan>(RemainingDurationVar, out var remainingDuration) || remainingDuration <= TimeSpan.Zero)
				return;

			foreach (var spreadTarget in spreadTargets)
				spreadTarget.StartBuff(BuffId.Virus_Debuff, buff.NumArg1, damage, remainingDuration, caster, buff.SkillId);
		}
	}
}
