using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
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
	public class Virus_DebuffOverride : DamageOverTimeBuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		private const int MaxSpreadOnDeathAmount = 2;
		private const float SpreadRange = 50f;
		private const float SpreadOnHitChance = 20f;
		private const int SpreadOnHitCount = 1;

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead)
			{
				if (!buff.Vars.ActivateOnce("Spread"))
					this.SpreadVirusOnDeath(buff);

				return;
			}

			base.WhileActive(buff);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}

		/// <summary>
		/// When the poisoned target is struck, 20% chance to spread
		/// poison to one nearby enemy with remaining duration.
		/// </summary>
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
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
			var caster = (ICombatEntity)buff.Caster;
			var target = buff.Target;

			var targetsInRange = target.Map.GetAttackableEnemiesInPosition(caster, target.Position, SpreadRange);
			var spreadTargets = targetsInRange.Where(a => !a.IsBuffActive(BuffId.Virus_Debuff));

			var damage = 0f;
			if (buff.Vars.TryGetFloat("Melia.DoT.SnapshotDamage", out var snapshotDamage))
				damage = snapshotDamage;

			foreach (var spreadTarget in spreadTargets.LimitRandom(MaxSpreadOnDeathAmount))
				spreadTarget.StartBuff(BuffId.Virus_Debuff, buff.NumArg1, damage, buff.Duration, caster, buff.SkillId);
		}
	}
}
