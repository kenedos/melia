using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Buffs;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handler for the Fletcher skill Fletcher Arrow Shot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fletcher_FletcherArrowShot)]
	public class Fletcher_FletcherArrowShotOverride : IForceSkillHandler
	{
		public const int QuiverBaseSize = 11;
		public const int BodkinCost = 2;
		public const int BarbedCost = 3;
		public const int SingijeonCost = 4;
		public const int CrossFireCost = 6;

		public static int GetQuiverSize(ICombatEntity caster)
		{
			if (caster.TryGetSkill(SkillId.Fletcher_FletcherArrowShot, out var fas))
				return QuiverBaseSize + fas.Level;
			return QuiverBaseSize;
		}

		public static int GetUsedSpace(ICombatEntity caster)
		{
			var used = 0;
			if (caster.TryGetBuff(BuffId.Fletcher_BodkinPoint_Buff, out var b1))
				used += b1.OverbuffCounter * BodkinCost;
			if (caster.TryGetBuff(BuffId.Fletcher_BarbedArrow_Buff, out var b2))
				used += b2.OverbuffCounter * BarbedCost;
			if (caster.TryGetBuff(BuffId.Fletcher_Singijeon_Buff, out var b3))
				used += b3.OverbuffCounter * SingijeonCost;
			if (caster.TryGetBuff(BuffId.Fletcher_CrossFire_Buff, out var b4))
				used += b4.OverbuffCounter * CrossFireCost;
			return used;
		}

		public static bool HasQuiverSpace(ICombatEntity caster, int cost)
		{
			return GetQuiverSize(caster) - GetUsedSpace(caster) >= cost;
		}

		private static readonly (SkillId skillId, AbilityId abilityId, BuffId buffId, int cost)[] ArrowsByPriority = new[]
		{
			(SkillId.Fletcher_CrossFire, AbilityId.Fletcher45, BuffId.Fletcher_CrossFire_Buff, CrossFireCost),
			(SkillId.Fletcher_Singijeon, AbilityId.Fletcher46, BuffId.Fletcher_Singijeon_Buff, SingijeonCost),
			(SkillId.Fletcher_BarbedArrow, AbilityId.Fletcher44, BuffId.Fletcher_BarbedArrow_Buff, BarbedCost),
			(SkillId.Fletcher_BodkinPoint, AbilityId.Fletcher43, BuffId.Fletcher_BodkinPoint_Buff, BodkinCost),
		};

		public static void TryFillQuiver(ICombatEntity caster)
		{
			foreach (var (skillId, abilityId, buffId, cost) in ArrowsByPriority)
			{
				if (caster.TryGetActiveAbility(abilityId, out _))
					continue;

				if (!caster.TryGetSkill(skillId, out var skill) || skill.IsOnCooldown)
					continue;

				if (!HasQuiverSpace(caster, cost))
					continue;

				skill.OnCooldownChanged?.Invoke();
				break;
			}
		}

		/// <summary>
		/// Handles the skill, shoot missile at enemy that spreads to another target.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var buffComponent = caster.Components.Get<BuffComponent>();
			Buff selectedBuff = null;

			if (buffComponent != null)
			{
				if (caster.TryGetBuff(BuffId.Fletcher_CrossFire_Buff, out var crossFireBuff))
					selectedBuff = crossFireBuff;
				else if (caster.TryGetBuff(BuffId.Fletcher_Singijeon_Buff, out var singijeonBuff))
					selectedBuff = singijeonBuff;
				else if (caster.TryGetBuff(BuffId.Fletcher_BarbedArrow_Buff, out var barbedBuff))
					selectedBuff = barbedBuff;
				else if (caster.TryGetBuff(BuffId.Fletcher_BodkinPoint_Buff, out var bodkinBuff))
					selectedBuff = bodkinBuff;
			}

			if (selectedBuff == null)
			{
				caster.ServerMessage(Localization.Get("No arrows found."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (target == null || !caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			Skill buffSkill;
			switch (selectedBuff.Id)
			{
				case BuffId.Fletcher_BarbedArrow_Buff:
					caster.TryGetSkill(SkillId.Fletcher_BarbedArrow, out buffSkill);
					break;
				case BuffId.Fletcher_BodkinPoint_Buff:
					caster.TryGetSkill(SkillId.Fletcher_BodkinPoint, out buffSkill);
					break;
				case BuffId.Fletcher_CrossFire_Buff:
					caster.TryGetSkill(SkillId.Fletcher_CrossFire, out buffSkill);
					break;
				case BuffId.Fletcher_Singijeon_Buff:
					caster.TryGetSkill(SkillId.Fletcher_Singijeon, out buffSkill);
					break;
				default:
					Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
					return;
			}

			if (buffSkill == null)
			{
				caster.StopBuff(selectedBuff.Id);
				return;
			}

			if (!caster.TrySpendSp(buffSkill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			selectedBuff.DecreaseOverbuff();
			if (selectedBuff.OverbuffCounter <= 0)
				caster.StopBuff(selectedBuff.Id);
			else
				Send.ZC_BUFF_UPDATE(caster, selectedBuff);
			if (buffSkill.Id == SkillId.Fletcher_BarbedArrow)
				this.HandleBarbedArrow(skill, buffSkill, caster, target);
			else if (buffSkill.Id == SkillId.Fletcher_BodkinPoint)
				this.HandleBodkinPoint(skill, buffSkill, caster, target);
			else if (buffSkill.Id == SkillId.Fletcher_CrossFire)
				this.HandleCrossFire(skill, buffSkill, caster, target);
			else if (buffSkill.Id == SkillId.Fletcher_Singijeon)
				this.HandleDivineMachineArrow(skill, buffSkill, caster, target);

			TryFillQuiver(caster);
		}

		private static float GetFasBonus(Skill fletcherSkill)
		{
			return fletcherSkill.Properties.GetFloat(PropertyName.SkillFactor) / 100f;
		}

		private void HandleBarbedArrow(Skill fletcherSkill, Skill buffSkill, ICombatEntity caster, ICombatEntity target)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_NORMAL.PlayForceEffect(caster, caster, target, forceId, "I_arrow005_mash2#Dummy_buff_L_hand", 0.7f, "arrow_cast", null, 1f, "arrow_blow", "SLOW", 800f, 1, 5, 10, 0);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, fletcherSkill, forceId, null);

			var modifier = new SkillModifier();
			modifier.DamageMultiplier += GetFasBonus(fletcherSkill);
			var skillHitResult = SCR_SkillHit(caster, target, buffSkill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var hit = new HitInfo(caster, target, buffSkill, skillHitResult);
			Send.ZC_HIT_INFO(caster, target, hit);

			if (skillHitResult.Damage > 0)
			{
				var bleedRate = 0.20f + 0.01f * buffSkill.Level;
				var bleedDamage = Math.Max(1, skillHitResult.Damage * bleedRate);
				target.StartBuff(BuffId.BroadHead_Debuff, 0, bleedDamage, TimeSpan.FromSeconds(6), caster, buffSkill.Id);
			}
		}

		private void HandleBodkinPoint(Skill fletcherSkill, Skill buffSkill, ICombatEntity caster, ICombatEntity target)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_NORMAL.PlayForceEffect(caster, caster, target, forceId, "I_arrow009_red#Dummy_q_Force", 0.7f, "arrow_cast", null, 1f, "arrow_blow", "SLOW", 800f, 1, 5, 10, 0);
			Send.ZC_NORMAL.PlayEffectNode(caster, "F_archer_shot_light_red", 0.6f, "Dummy_arrow_effect", "None");
			Send.ZC_SKILL_FORCE_TARGET(caster, target, fletcherSkill, forceId, null);

			var defPenRate = 0.10f + 0.01f * buffSkill.Level;
			var modifier = SkillModifier.MultiHit(2);
			modifier.DefensePenetrationRate = defPenRate;
			modifier.DamageMultiplier += GetFasBonus(fletcherSkill);

			var skillHitResult = SCR_SkillHit(caster, target, buffSkill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);

			var hit = new HitInfo(caster, target, buffSkill, skillHitResult);
			Send.ZC_HIT_INFO(caster, target, hit);
		}

		private void HandleCrossFire(Skill fletcherSkill, Skill buffSkill, ICombatEntity caster, ICombatEntity target)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_NORMAL.PlayForceEffect(caster, caster, target, forceId, "I_arrow005_mash2_red#Dummy_buff_L_hand", 0.7f, null, "F_archer_crossarrow_shot_ground", 1f, null, "FAST", 500f, 0, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(caster, target.Position, "E_archer_crossarrow_shot_ground", 1.3f, 0, 0.2f);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, fletcherSkill, forceId, null);

			var targetPos = target.Position;

			var dirX = new Direction(0);
			var splashAreaX = SplashAreas.Square.Centered(targetPos, dirX, 200, 30);
			var targetsX = caster.Map.GetAttackableEnemiesIn(caster, splashAreaX);

			var dirZ = new Direction(90);
			var splashAreaZ = SplashAreas.Square.Centered(targetPos, dirZ, 200, 30);
			var targetsZ = caster.Map.GetAttackableEnemiesIn(caster, splashAreaZ);

			var hitTargetsX = targetsX.LimitBySDR(caster, buffSkill);
			foreach (var aoeTarget in hitTargetsX)
			{
				var modifier = new SkillModifier();
				modifier.DamageMultiplier += GetFasBonus(fletcherSkill);

				var skillHitResult = SCR_SkillHit(caster, aoeTarget, buffSkill, modifier);
				aoeTarget.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, aoeTarget, buffSkill, skillHitResult);
				Send.ZC_HIT_INFO(caster, aoeTarget, hit);
			}

			var hitTargetsZ = targetsZ.LimitBySDR(caster, buffSkill);
			foreach (var aoeTarget in hitTargetsZ)
			{
				var modifier = new SkillModifier();
				modifier.DamageMultiplier += GetFasBonus(fletcherSkill);

				var skillHitResult = SCR_SkillHit(caster, aoeTarget, buffSkill, modifier);
				aoeTarget.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, aoeTarget, buffSkill, skillHitResult);
				Send.ZC_HIT_INFO(caster, aoeTarget, hit);
			}
		}

		private void HandleDivineMachineArrow(Skill fletcherSkill, Skill buffSkill, ICombatEntity caster, ICombatEntity target)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_NORMAL.PlayForceEffect(caster, caster, target, forceId, "E_archer_Flareshot_arrow_violet#Dummy_q_Force", 0.3f, null, "", 0.5f, null, "FAST", 700f, 0, 0, 0, 0);
			Send.ZC_GROUND_EFFECT(caster, target.Position, "I_explosion002_orange", 1.5f, 0, 0.2f);
			Send.ZC_GROUND_EFFECT(caster, target.Position, "F_archer_caltrop_hit_explosion", 1f, 0, 0.1f);
			Send.ZC_SKILL_FORCE_TARGET(caster, target, fletcherSkill, forceId, null);

			var splashArea = new Circle(target.Position, 60);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).LimitBySDR(caster, buffSkill);

			foreach (var aoeTarget in targets)
			{
				var modifier = new SkillModifier();
				modifier.DamageMultiplier += GetFasBonus(fletcherSkill);
				var skillHitResult = SCR_SkillHit(caster, aoeTarget, buffSkill, modifier);
				aoeTarget.TakeDamage(skillHitResult.Damage, caster);

				var hit = new HitInfo(caster, aoeTarget, buffSkill, skillHitResult);
				Send.ZC_HIT_INFO(caster, aoeTarget, hit);
			}
		}
	}
}
