using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.SplashAreas;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World;
using Melia.Zone.Skills.Handlers.Common;

namespace Melia.Zone.Skills.HandlersOverrides.Wizards.Psychokino
{
	/// <summary>
	/// Handler for the Psychokino skill Magnetic Force.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Psychokino_MagneticForce)]
	public class Psychokino_MagneticForceOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int LifeTimeSeconds = 3;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_wiz_magneticforce_shot", "voice_wiz_m_magneticforce_shot");
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public async void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var totalRadius = 100f;
			var area = new CircleF(targetPos, totalRadius);
			var allEnemiesInArea = caster.Map.GetAttackableEnemiesIn(caster, area).ToList();

			if (!allEnemiesInArea.Any())
				return;

			var magnetCount = 4;
			var sectorAngle = 360f / magnetCount;

			for (var i = 0; i < magnetCount; i++)
			{
				var startAngle = i * sectorAngle;
				var endAngle = (i + 1) * sectorAngle;

				var enemiesInSector = allEnemiesInArea.Where(enemy =>
				{
					var enemyDirection = targetPos.GetDirection(enemy.Position);
					var enemyAngle = enemyDirection.DegreeAngle;
					if (enemyAngle < 0)
						enemyAngle += 360;

					return enemyAngle >= startAngle && enemyAngle < endAngle;
				}).ToList();

				Position magnetPos;
				if (enemiesInSector.Any())
				{
					var avgX = enemiesInSector.Average(e => e.Position.X);
					var avgZ = enemiesInSector.Average(e => e.Position.Z);
					magnetPos = new Position((float)avgX, targetPos.Y, (float)avgZ);
				}
				else
				{
					var midAngle = startAngle + (sectorAngle / 2);
					var subAreaRadius = totalRadius * 0.6f;
					magnetPos = targetPos.GetRelative(new Direction(midAngle), subAreaRadius);
				}

				var magneticMob = this.CreateMagneticMonster(caster, skill, magnetPos, "HiddenFlameSeed", LifeTimeSeconds, 50);
				if (magneticMob == null || magneticMob.Trigger == null)
					return;

				magneticMob.Trigger.MaxActorCount = 5;
				magneticMob.Trigger.UpdateInterval = TimeSpan.FromMilliseconds(300);
				magneticMob.Trigger.LifeTime = TimeSpan.FromSeconds(LifeTimeSeconds);
				magneticMob.Trigger.Subscribe(TriggerType.Update, this.OnTriggerUpdate);
			}
		}

		private void OnTriggerUpdate(object? sender, TriggerArgs e)
		{
			if (e.Trigger is Mob mob)
				this.UpdateMagneticForce(mob);
		}

		/// <summary>
		/// Updates the magnetic force effect on monsters within the specified range.
		/// </summary>
		/// <param name="magneticMob">The entity controlling the magnetic effect.</param>
		public void UpdateMagneticForce(Mob magneticMob)
		{
			if (magneticMob == null || magneticMob.Vars == null) return;

			var skillLevel = magneticMob.Vars.GetInt("SKILL_LV");
			var caster = magneticMob.Vars.Get<ICombatEntity>("SKILL_OWNER");
			var range = magneticMob.Vars.GetFloat("ATTR_RANGE");
			var lifeTime = magneticMob.Vars.GetFloat("LIFE_TIME");

			if (caster == null || caster.Map == null) return;
			if (!caster.TryGetSkill(SkillId.Psychokino_MagneticForce, out var skill)) return;

			var stunChance = 0f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Psychokino4, out var abilityLevel))
				stunChance = abilityLevel * 0.05f;

			var nearbyTargets = caster.Map.GetAttackableEnemiesInPosition(caster, magneticMob.Position, range);
			var targetsToHit = nearbyTargets.Take(5);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targetsToHit)
			{
				if (target == null || target == magneticMob) continue;

				if (target.IsKnockdownable())
				{
					var pullDirection = target.Position.GetDirection(magneticMob.Position);
					var pullFromPos = target.Position.GetRelative(pullDirection.Backwards, 50);

					var skillHitResult = new SkillHitResult { Damage = 0, Result = HitResultType.Hit };
					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);
					skillHit.KnockBackInfo = new KnockBackInfo(pullFromPos, target, HitType.KnockBack, 80, 10);
					skillHit.KnockBackInfo.Speed = 1;
					skillHit.KnockBackInfo.VPow = 1;
					skillHit.HitInfo.Type = HitType.KnockBack;

					target.ApplyKnockback(caster, skill, skillHit);
					hits.Add(skillHit);
				}

				if (targetsToHit.Count() >= 1)
				{
					target.StartBuff(BuffId.MagneticForce_Debuff, skillLevel, (int)HitType.Normal, TimeSpan.FromMilliseconds(300), caster);

					if (stunChance > 0 && RandomProvider.Get().NextDouble() < stunChance)
					{
						target.StartBuff(BuffId.Stun, 1, 0, TimeSpan.FromMilliseconds(1500), caster);
					}
				}
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Create a magnetic entity (monster) with specific properties and effects.
		/// </summary>
		/// <param name="lifeTime">in seconds</param>
		private Mob CreateMagneticMonster(ICombatEntity caster, Skill skill, Position position, string monsterName,
			float lifeTime, float attrRange)
		{
			var magneticMob = MonsterSkillCreateMob(skill, caster, monsterName, position, 0, "", "None", 0, lifeTime, "None", "Faction#Trap");

			if (magneticMob == null)
			{
				Log.Debug("CreateMagneticMonster: Failed to create monster {0}.", monsterName);
				return null;
			}

			magneticMob.SetHittable(false);
			magneticMob.MoveType = MoveType.Holding;

			//magneticMob.PlayEffectToGround(attachEffect, position, effectScale, lifeTime);
			caster.PlayEffectToGround("F_wizard_magneticforce_shot_ground2", position, 0.5f, LifeTimeSeconds * 1000);

			magneticMob.Vars.Set("SKILL_OWNER", caster);
			magneticMob.Vars.Set("SKILL_LV", skill.Level);
			magneticMob.Vars.Set("SKILL_SR", skill.SkillSR);
			magneticMob.Vars.Set("LIFE_TIME", lifeTime);
			magneticMob.Vars.Set("ATTR_RANGE", attrRange);
			magneticMob.Vars.Set("TICK_COUNT", 0f);

			magneticMob.Components.Add(magneticMob.Trigger = new TriggerComponent(magneticMob, new CircleF(magneticMob.Position, attrRange)));

			return magneticMob;
		}
	}
}
