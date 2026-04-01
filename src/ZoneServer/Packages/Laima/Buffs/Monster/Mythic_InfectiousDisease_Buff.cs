using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers.Monster;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Laima.Monster
{
	/// <summary>
	/// Handler for Mythic_InfectiousDisease_Buff.
	/// Monster periodically infects a random nearby player with a disease
	/// debuff. Infected players spread it to others standing close together.
	/// Damage is snapshotted via SCR_SkillHit at time of infection.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mythic_InfectiousDisease_Buff)]
	public class Mythic_InfectiousDisease_BuffOverride : BuffHandler
	{
		private const int InfectIntervalMs = 1000;
		private const float InfectRange = 50f;
		private const int DebuffDurationSec = 10;
		private const float DamageRate = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Mob monster)
				return;

			MythicBuffHelper.ApplyMythicStats(monster);
			buff.SetUpdateTime(InfectIntervalMs);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Mob monster || monster.Map == null || monster.IsDead)
				return;

			var enemies = monster.Map.GetAttackableEnemiesInPosition(monster, monster.Position, InfectRange);
			if (enemies.Count == 0)
				return;

			var rnd = RandomProvider.Get();
			var target = enemies[rnd.Next(enemies.Count)];

			if (target.TryGetBuff(BuffId.Mythic_InfectiousDisease_Debuff, out var existing) && existing.RemainingDuration >= TimeSpan.FromSeconds(6))
				return;

			var dummySkill = new Skill(monster, SkillId.Normal_Attack);
			var skillHitResult = SCR_SkillHit(monster, target, dummySkill);
			var snapshotDamage = skillHitResult.Damage * DamageRate;

			target.StartBuff(BuffId.Mythic_InfectiousDisease_Debuff, 1, snapshotDamage, TimeSpan.FromSeconds(DebuffDurationSec), monster);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
