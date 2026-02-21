using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper skill Claymore.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sapper_Claymore)]
	public class Sapper_ClaymoreOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float ClaymoreLifetimeSeconds = 120f;
		private const float ExplosionRadius = 70f;
		private const float SpawnDistance = 22.4f;
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			skill.IncreaseOverheat();

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			var spawnPos = originPos.GetRelative(caster.Direction, distance: SpawnDistance);

			var claymore = MonsterSkillCreateMob(skill, caster, "skill_sapper_trap4", spawnPos, 0, "", "", 0, ClaymoreLifetimeSeconds, "MON_DUMMY", "");
			if (claymore != null)
			{
				claymore.Vars.Set("Skill", skill);
				claymore.Vars.Set("Caster", caster);
				claymore.StartBuff(BuffId.Cover_Buff, TimeSpan.FromSeconds(ClaymoreLifetimeSeconds), caster);
				claymore.Died += this.Claymore_Died;
			}

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			//if (caster is Character character)
			//	Send.ZC_NORMAL.Skill_122(character, claymore, "SAPPER_TRAP_Sapper_Claymore");
		}

		private void Claymore_Died(Mob claymore, ICombatEntity killer)
		{
			var caster = claymore.Vars.Get<ICombatEntity>("Caster");
			var skill = claymore.Vars.Get<Skill>("Skill");
			var detonateTrapsSkill = claymore.Vars.Get<Skill>("DetonateTrapsSkill");

			if (skill == null || caster == null)
				return;

			if (detonateTrapsSkill == null)
				return;

			var explosionPos = claymore.Position;

			Send.ZC_GROUND_EFFECT(claymore, explosionPos, "F_explosion006_orange1", 1f, 0f, 0f);

			var splashArea = new Circle(explosionPos, ExplosionRadius);

			this.Attack(skill, caster, splashArea, explosionPos, detonateTrapsSkill);
		}

		private void Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea, Position explosionPos, Skill detonateTrapsSkill = null)
		{
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var damageDelay = TimeSpan.FromMilliseconds(150);

			var modifier = SkillModifier.MultiHit(3);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.Take(5))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult);

				if (detonateTrapsSkill != null && detonateTrapsSkill.Level >= 2)
				{
					if (skillHitResult.Damage > 0 && target.IsKnockdownable())
					{
						var velocity = 20 + 20 * detonateTrapsSkill.Level;
						skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockDown, velocity, 70);
						skillHit.HitInfo.Type = HitType.KnockDown;
						target.ApplyKnockdown(caster, skill, skillHit);
					}
				}

				if (detonateTrapsSkill != null && detonateTrapsSkill.Level >= 5)
				{
					if (skillHitResult.Damage > 0)
					{
						var burnDamagePercent = 0.1f * (detonateTrapsSkill.Level - 4);

						if (caster.TryGetActiveAbilityLevel(AbilityId.Sapper14, out var abilityLevel))
							burnDamagePercent *= 1 + abilityLevel * 0.005f;

						var burnDamage = skillHitResult.Damage * burnDamagePercent;
						target.StartBuff(BuffId.Fire, detonateTrapsSkill.Level, burnDamage, TimeSpan.FromSeconds(4), caster);
					}
				}

				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
