using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Blazing Arrow.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_BlazingArrow)]
	public class RangerBlazingArrowOverride : IForceSkillHandler
	{
		private const float BaseJumpDistance = 30f;
		private const float JumpDistancePerLevel = 3f;
		private const float JumpDistanceEvasionMultiplierPerLevel = 0.02f;
		private const float BurnDurationSeconds = 4;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			if (!caster.InSkillUseRange(skill, target))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			skill.Run(this.Attack(skill, caster, target));
		}

		/// <summary>
		/// Performs the actual attack.
		/// </summary>
		public async Task Attack(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var jumpDelay = TimeSpan.FromMilliseconds(600);
			var animationDelay = TimeSpan.FromMilliseconds(400);
			var skillHitDelay = TimeSpan.Zero;

			var modifier = SkillModifier.MultiHit(4);

			var isIceVariant = false;
			var animationName = "I_force009_fire2";

			if (caster.IsAbilityActive(AbilityId.Ranger38))
			{
				isIceVariant = true;
				animationName = "I_arrow003_blue";

				modifier.AttackAttribute = AttributeType.Ice;
				modifier.DamageMultiplier -= 0.3f;
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position);

			var targetPos = caster.Position.GetRelative(caster.Direction.Backwards, this.GetJumpDistance(caster, skill));
			targetPos = caster.Map.Ground.GetLastValidPosition(caster.Position, targetPos);

			if (caster is Character character)
			{
				if (!character.IsWearingArmorOfType(ArmorMaterialType.Iron))
				{
					caster.Position = targetPos;
					Send.ZC_NORMAL.LeapJump(caster, targetPos, 2f, 0.1f, 0.1f, 0.2f, 0.1f, 3);
				}
			}

			await skill.Wait(jumpDelay);

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			var damage = skillHitResult.Damage;
			target.TakeDamage(damage, caster);

			var hit = new HitInfo(caster, target, skill, skillHitResult);
			hit.ForceId = ForceId.GetNew();
			hit.ResultType = HitResultType.NoHitScript;

			Send.ZC_NORMAL.PlayForceEffect(hit.ForceId, caster, caster, target, animationName, 1.3f, "arrow_cast", "F_hit_good", 1, "arrow_blow", "SLOW", 400);

			var damageDelay = TimeSpan.FromMilliseconds(0);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
			//Send.ZC_HIT_INFO(caster, target, hit);

			// If the target has Scan, it is removed, but the cooldown is cut
			// by 15 seconds
			if (target.IsBuffActive(BuffId.Ranger_Scan_Debuff))
			{
				target.StopBuff(BuffId.Ranger_Scan_Debuff);
				skill.ReduceCooldown(TimeSpan.FromSeconds(15));
			}

			if (isIceVariant)
			{
				var duration = TimeSpan.FromSeconds(3);
				target.StartBuff(BuffId.Freeze, skill.Level, 0, duration, caster);
			}
			else if (damage > 0)
			{
				var seconds = BurnDurationSeconds;
				var duration = TimeSpan.FromSeconds(seconds);
				damage = Math.Max(1, damage / seconds);
				target.StartBuff(BuffId.Fire, skill.Level, damage, duration, caster);
			}

			await skill.Wait(animationDelay);

			Ranger_CriticalShotOverride.TryActivateDoubleTake(skill, caster, target);
			Ranger_StrafeOverride.TryApplyStrafeBuff(caster);
		}

		/// <summary>
		/// Returns the distance to jump back.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float GetJumpDistance(ICombatEntity caster, Skill skill)
		{
			var evasion = caster.Properties.GetFloat(PropertyName.DR);
			var level = skill.Level;

			var distance = BaseJumpDistance + JumpDistancePerLevel * level;

			distance += JumpDistanceEvasionMultiplierPerLevel * level * evasion;

			return Math.Min(120, distance);
		}
	}
}
