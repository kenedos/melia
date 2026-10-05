using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Buffs.Handlers.Monster
{
	/// <summary>
	/// Shared helper for mythic monster buffs.
	/// </summary>
	public static class MythicBuffHelper
	{
		private const float ShieldHpRate = 20f;
		private const int MinionCount = 5;
		private const int RespawnDelayMs = 30000;
		private const string RespawnTimeVar = "Melia.Mythic.RespawnTime";
		/// <summary>
		/// Applies mythic stat boosts to a monster (size, rank, HP/SP/ATK/DEF/speed).
		/// </summary>
		public static void ApplyMythicStats(Mob monster)
		{
			var worldConf = ZoneServer.Instance.Conf.World;

			var size = monster.EffectiveSize;
			if (size == SizeType.S)
			{
				monster.Properties.SetString(PropertyName.Size, SizeType.L);
				monster.InvalidateSizeCache();
				monster.ChangeScale(2f, 1f);
			}
			else if (size == SizeType.M)
			{
				monster.Properties.SetString(PropertyName.Size, SizeType.L);
				monster.InvalidateSizeCache();
				monster.ChangeScale(1.5f, 1f);
			}

			if (monster.Rank == MonsterRank.Normal)
				monster.Rank = MonsterRank.Elite;

			var prevMaxHP = monster.Properties.GetFloat(PropertyName.MHP);
			var newMaxHP = prevMaxHP * worldConf.MythicHPSPRate / 100f;
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.MHP, newMaxHP);
			propertyOverrides.Add(PropertyName.MSP, monster.Properties.GetFloat(PropertyName.MSP) * worldConf.MythicHPSPRate / 100f);
			var statRate = worldConf.MythicStatRate / 100f;
			propertyOverrides.Add(PropertyName.MINPATK, monster.Properties.GetFloat(PropertyName.MINPATK) * statRate);
			propertyOverrides.Add(PropertyName.MAXPATK, monster.Properties.GetFloat(PropertyName.MAXPATK) * statRate);
			propertyOverrides.Add(PropertyName.MINMATK, monster.Properties.GetFloat(PropertyName.MINMATK) * statRate);
			propertyOverrides.Add(PropertyName.MAXMATK, monster.Properties.GetFloat(PropertyName.MAXMATK) * statRate);
			propertyOverrides.Add(PropertyName.DEF, monster.Properties.GetFloat(PropertyName.DEF) * statRate);
			propertyOverrides.Add(PropertyName.MDEF, monster.Properties.GetFloat(PropertyName.MDEF) * statRate);
			propertyOverrides.Add(PropertyName.CRTHR, monster.Properties.GetFloat(PropertyName.CRTHR) * statRate);
			propertyOverrides.Add(PropertyName.CRTATK, monster.Properties.GetFloat(PropertyName.CRTATK) * statRate);
			propertyOverrides.Add(PropertyName.CRTDR, monster.Properties.GetFloat(PropertyName.CRTDR) * statRate);
			propertyOverrides.Add(PropertyName.DR, monster.Properties.GetFloat(PropertyName.DR) * statRate);
			propertyOverrides.Add(PropertyName.HR, monster.Properties.GetFloat(PropertyName.HR) * statRate);

			var runSpeed = monster.Properties.GetFloat(PropertyName.RunMSPD);
			var baseRunSpeed = 60;
			var additionalRunSpeed = Math.Max(3, runSpeed - baseRunSpeed);
			propertyOverrides.Add(PropertyName.RunMSPD, runSpeed + (additionalRunSpeed * statRate * 0.5f));

			monster.ApplyOverrides(propertyOverrides);

			// +2 SDR for mythic monsters
			monster.Properties.Modify(PropertyName.SDR_BM, 2);

			// 30% faster skill usage
			monster.Vars.Set("Melia.ShootTimeMultiplier", 0.7f);
			monster.Vars.Set("Melia.Mythic.IsMythic", true);
			monster.Heal(newMaxHP - prevMaxHP, 0);
			monster.InvalidateProperties();

			monster.Properties.SetFloat(PropertyName.ShieldRate, ShieldHpRate);
			monster.Properties.Invalidate(PropertyName.MShield);
			monster.Shield = monster.MaxShield;
			monster.Properties.AutoUpdateMax(PropertyName.Shield, PropertyName.MShield);
		}

		/// <summary>
		/// Spawns minions using MonsterSkillCreateMob (same as boss summons).
		/// Uses the leader's own class name so minions are the same monster type.
		/// </summary>
		public static List<Mob> SpawnMythicMinions(Mob leader)
		{
			return SpawnMythicMinions(leader, MinionCount);
		}

		/// <summary>
		/// Spawns a specific number of minions using MonsterSkillCreateMob.
		/// </summary>
		public static List<Mob> SpawnMythicMinions(Mob leader, int count)
		{
			var minions = new List<Mob>();

			if (leader.Map == null || leader.IsDead)
				return minions;

			var className = leader.Data?.ClassName;
			if (string.IsNullOrEmpty(className))
				return minions;

			var dummySkill = new Skill(leader, SkillId.Normal_Attack);

			for (var i = 0; i < count; i++)
			{
				var pos = leader.Position.GetRandomInRange2D(30, 80);
				if (!leader.Map.Ground.TryGetNearestValidPosition(pos, out var validPos))
					validPos = leader.Position;

				var minion = MonsterSkillCreateMob(dummySkill, leader, className, validPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
				if (minion != null)
				{
					minion.StartBuff(BuffId.EliteMonsterSummonBuff, 1, 0, TimeSpan.FromSeconds(15), leader);
					minions.Add(minion);
				}
			}

			return minions;
		}

		/// <summary>
		/// Maintains minion count for a mythic monster, cleaning up dead
		/// minions and respawning after a 30-second delay.
		/// </summary>
		public static void MaintainMinions(Buff buff, Mob leader, List<int> minionHandles, int targetCount = MinionCount)
		{
			if (leader.Map == null || leader.IsDead)
				return;

			// Remove dead/gone minions from tracking
			var hadDeaths = false;
			for (var i = minionHandles.Count - 1; i >= 0; i--)
			{
				if (!leader.Map.TryGetCombatEntity(minionHandles[i], out var minion) || minion.IsDead)
				{
					minionHandles.RemoveAt(i);
					hadDeaths = true;
				}
			}

			// Start respawn timer when minions die
			if (hadDeaths && (!buff.Vars.TryGet<long>(RespawnTimeVar, out var existingTime) || existingTime == 0))
				buff.Vars.Set(RespawnTimeVar, Environment.TickCount64 + RespawnDelayMs);

			// Spawn more if below target count and delay has elapsed
			var needed = targetCount - minionHandles.Count;
			if (needed > 0)
			{
				if (buff.Vars.TryGet<long>(RespawnTimeVar, out var respawnTime) && respawnTime > 0 && Environment.TickCount64 < respawnTime)
					return;

				buff.Vars.Set(RespawnTimeVar, 0L);
				var newMinions = SpawnMythicMinions(leader, needed);
				foreach (var m in newMinions)
					minionHandles.Add(m.Handle);
			}
		}

	}
}
