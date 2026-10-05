using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers.Monster;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_Bomb_Buff.
	/// Monster periodically applies bomb debuff to a random nearby player.
	/// Monster explodes on death dealing AoE damage.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_Bomb_Buff)]
	public class Mythic_Bomb_BuffOverride : BuffHandler
	{
		private const string DeathSubscribedVar = "Melia.Mythic.BombDeath";
		private const int BombIntervalMs = 10000;
		private const float BombApplyRange = 120f;
		private const int BombDebuffDurationSec = 15;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Mob monster)
				return;

			MythicBuffHelper.ApplyMythicStats(monster);

			buff.SetUpdateTime(BombIntervalMs);
			monster.Died += OnMonsterDied;
			buff.Vars.Set(DeathSubscribedVar, true);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Mob monster || monster.Map == null || monster.IsDead)
				return;

			var enemies = monster.Map.GetAttackableEnemiesInPosition(monster, monster.Position, BombApplyRange);
			if (enemies.Count == 0)
				return;

			foreach (var enemy in enemies)
			{
				enemy.StartBuff(BuffId.Mythic_Bomb_Def_Debuff, 1, 0, TimeSpan.FromSeconds(BombDebuffDurationSec), monster);
			}

		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Mob monster && buff.Vars.GetBool(DeathSubscribedVar))
			{
				monster.Died -= OnMonsterDied;
			}
		}

		private static void OnMonsterDied(Mob mob, ICombatEntity killer)
		{
			mob.Died -= OnMonsterDied;

			if (mob.Map == null)
				return;

			// Explodes when dies
			var enemies = mob.Map.GetAttackableEnemiesInPosition(mob, mob.Position, 75);
			var dummySkill = new Skill(mob, SkillId.Normal_Attack);
			var aniTime = TimeSpan.FromMilliseconds(0);

			Send.ZC_NORMAL.PlayEffect(mob, "F_archer_explosiontrap_hit_explosion", 1.5f);

			var hits = new List<SkillHitInfo>();

			foreach (var enemy in enemies)
			{
				var skillHitResult = SCR_SkillHit(mob, enemy, dummySkill);
				var overbuffCounter = 0;
				if (enemy.TryGetBuff(BuffId.Mythic_Bomb_Def_Debuff, out var bombDebuff))
					overbuffCounter = bombDebuff.OverbuffCounter;
				skillHitResult.Damage *= (1f + overbuffCounter * 0.2f);

				if (skillHitResult.Damage <= 0)
					continue;

				enemy.TakeDamage(skillHitResult.Damage, mob);

				var skillHit = new SkillHitInfo(mob, enemy, dummySkill, skillHitResult, aniTime, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;

				if (enemy.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(mob, enemy, KnockBackType.KnockDown, 180, 60, KnockDirection.Random);
					skillHit.HitInfo.KnockBackType = KnockBackType.KnockDown;
					enemy.ApplyKnockdown(mob, dummySkill, skillHit);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(mob, hits);
		}
	}
}
