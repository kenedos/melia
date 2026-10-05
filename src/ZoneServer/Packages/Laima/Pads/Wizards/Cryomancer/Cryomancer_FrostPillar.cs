using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using System.Collections.Generic;
using Melia.Zone.Skills.Combat;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Cryomancer_FrostPillar)]
	public class Cryomancer_FrostPillarOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float PadRange = 120f;
		private const int BasePadLifeTimeMilliseconds = 8000;
		private const int PadLifeTimePerLevelMilliseconds = 1100;
		private const int UpdateInterval = 500;
		private const int FreezeDurationMilliSeconds = 4000;
		private const int BaseFreezeChance = 60;
		private const int FreezeChancePerLevel = 4;

		/// <summary>
		/// Handles the creation of the Frost Pillar pad.
		/// </summary>
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(PadRange);
			pad.SetUpdateInterval(UpdateInterval);
			var lifeTime = BasePadLifeTimeMilliseconds + (PadLifeTimePerLevelMilliseconds * pad.Skill.Level);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(lifeTime);
			pad.Trigger.MaxActorCount = 10;

			this.CreatePillarMonster(pad);
		}

		/// <summary>
		/// Handles the destruction of the Frost Pillar pad.
		/// </summary>
		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);
		}

		/// <summary>
		/// Handles an entity entering the Frost Pillar pad area.
		/// </summary>
		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			var buffTime = FreezeDurationMilliSeconds;
			this.ApplyFrostPillarBuff(pad, creator, initiator, buffTime);
		}

		/// <summary>
		/// Handles an entity leaving the Frost Pillar pad area.
		/// </summary>
		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			PadTargetBuffRemoveMonster(pad, initiator, RelationType.Enemy, 0, 0, BuffId.Gust_Debuff);
		}

		/// <summary>
		/// Handles periodic updates of the Frost Pillar pad.
		/// </summary>
		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			this.DealDamageToEnemies(pad);
			this.RefreshFrostPillarBuff(pad, creator, skill);
			this.ApplyFrostPillarDebuff(pad);
		}

		private void CreatePillarMonster(Pad pad)
		{
			var lifeTime = BasePadLifeTimeMilliseconds + (PadLifeTimePerLevelMilliseconds * pad.Skill.Level);
			var monster = PadCreateMonster(pad, "attract_pillar", pad.Position, 0f, 0, lifeTime, "", "None", 1, true, "None", "None", false, "SET_PVE_NODAMAGE");
			var mob = monster as Mob;
			mob.SetHittable(false);
			mob.MonsterType = RelationType.Friendly;
			mob.Faction = FactionType.Law;
			mob.StartBuff(BuffId.Invincible);
		}

		private void ApplyFrostPillarBuff(Pad pad, ICombatEntity creator, ICombatEntity target, float duration)
		{
			var freezeChance = BaseFreezeChance + FreezeChancePerLevel * pad.Skill.Level;

			if (creator.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			PadTargetBuffAfterBuffCheck(pad, target, RelationType.Enemy, 0, 0, BuffId.Cryomancer_Freeze, BuffId.Cryomancer_Freeze, 1, 0, (int)duration, 1, freezeChance, false);
		}

		private void DealDamageToEnemies(Pad pad)
		{
			var caster = pad.Creator as ICombatEntity;
			var skill = pad.Skill;
			var map = pad.Map;

			if (pad.IsDead || caster == null || caster.IsDead || skill == null
				|| map == null || caster.Map != map)
				return;

			var remainingTargets = pad.Trigger.MaxActorCount;
			var targets = map.GetAttackableEnemiesIn(caster, pad.Area);
			var hits = new List<SkillHitInfo>();

			foreach (var actor in targets)
			{
				if (remainingTargets <= 0 || pad.IsDead)
					break;

				if (actor is not ICombatEntity target || target.IsDead || !caster.IsEnemy(target))
					continue;

				var modifier = SkillModifier.MultiHit(1);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);

				if (!target.IsDead && skillHitResult.Result != HitResultType.Dodge
					&& target.IsKnockdownable()
					&& target.Position.Get2DDistance(pad.Position) > 1f)
				{
					var pullDirection = target.Position.GetDirection(pad.Position);
					var pullFromPosition = target.Position.GetRelative(pullDirection.Backwards, 120);

					skillHit.KnockBackInfo = new KnockBackInfo(pullFromPosition, target, KnockBackType.KnockBack, 150, 10);
					skillHit.KnockBackInfo.Speed = 1;
					skillHit.KnockBackInfo.VPow = 1;
					skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;

					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);
				remainingTargets--;
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private void RefreshFrostPillarBuff(Pad pad, ICombatEntity creator, Skill skill)
		{
			var buffTime = FreezeDurationMilliSeconds;
			var freezeChance = BaseFreezeChance + FreezeChancePerLevel * skill.Level;

			if (creator.TryGetActiveAbility(AbilityId.Cryomancer9, out var abilCryomancer9))
				freezeChance = (int)Math.Floor(freezeChance * (1 + abilCryomancer9.Level * 0.05));

			PadBuffCheckBuffEnemy(pad, RelationType.Enemy, 0, 0, BuffId.Cryomancer_Freeze, BuffId.Cryomancer_Freeze, 1, 0, buffTime, 1, freezeChance);
		}

		private void ApplyFrostPillarDebuff(Pad pad)
		{
			PadBuffEnemyMonster(pad, RelationType.Enemy, 0, 0, BuffId.FrostPillar_Debuff, 1, 0, 1, 1, BaseFreezeChance + FreezeChancePerLevel * pad.Skill.Level);
		}
	}
}
