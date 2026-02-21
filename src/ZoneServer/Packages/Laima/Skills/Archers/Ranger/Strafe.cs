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
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Ranger skill Strafe.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Ranger_Strafe)]
	public class Ranger_StrafeOverride : IMeleeGroundSkillHandler
	{
		private const float BaseStrafeDistance = 40f;
		private const float StrafeDistancePerLevel = 4f;
		private const float StrafeDistanceEvasionMultiplierPerLevel = 0.04f;

		/// <summary>
		/// Handles the skill
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			var target = targets.FirstOrDefault();
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target == null)
				return;
			
			skill.Run(this.Attack(skill, caster, target));
			
			// Then perform the jump
			if (caster is Character character)
			{
				if (!character.IsWearingArmorOfType(ArmorMaterialType.Iron))
				{
					var direction = caster.Direction;
					var targetPos = caster.Position.GetRelative(direction, this.GetStrafeDistance(caster, skill));
					targetPos = caster.Map.Ground.GetLastValidPosition(caster.Position, targetPos);

					caster.Position = targetPos;
					Send.ZC_NORMAL.LeapJump(caster, targetPos, 1.5f, 0.1f, 0.1f, 0.2f, 0.1f, 3);
				}
			}
		}

		/// <summary>
		/// Performs the actual attack.
		/// </summary>
		public async Task Attack(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			var animationDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			// Skill can only be used if the strafe buff is active
			if (!caster.TryGetBuff(BuffId.Ranger_StrapingShot, out var enableBuff))
				return;

			var direction = caster.Direction;
			var previousStrafes = enableBuff.NumArg1;
			caster.StopBuff(BuffId.Ranger_StrapingShot);

			// Now fire the arrow
			var modifier = SkillModifier.Default;

			if (caster.IsAbilityActive(AbilityId.Ranger54))
				modifier.DamageMultiplier += 1f;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);
			var hit = new HitInfo(caster, target, skill, skillHitResult);
			hit.ForceId = ForceId.GetNew();
			hit.ResultType = HitResultType.NoHitScript;

			Send.ZC_NORMAL.PlayForceEffect(hit.ForceId, caster, caster, target, "I_arrow009_red", 0.2f, "arrow_cast", "F_hit_good", 1, "arrow_blow", "SLOW", 800);
			Send.ZC_HIT_INFO(caster, target, hit);

			await skill.Wait(animationDelay);

			caster.TurnTowards(target);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position);

			Ranger_CriticalShotOverride.TryActivateDoubleTake(skill, caster, target);

			// Ranger54 lets you chain strafe up to 3 times,
			// but you don't get the evasion buff
			if (caster.IsAbilityActive(AbilityId.Ranger54))
			{
				if (previousStrafes < 2)
				{
					previousStrafes++;
					caster.StartBuff(BuffId.Ranger_StrapingShot, previousStrafes, 0, TimeSpan.FromSeconds(3), caster);
				}
			}
			else
			{
				if (skillHitResult.Result != HitResultType.Dodge)
					caster.StartBuff(BuffId.Ranger_StrapingShot_Dodge_Buff, skill.Level, 0, TimeSpan.FromSeconds(2), caster);
			}
		}

		/// <summary>
		/// Attempts to give the buff that allows the use of Strafe.
		/// </summary>
		public static void TryApplyStrafeBuff(ICombatEntity caster)
		{
			if (!caster.TryGetSkill(SkillId.Ranger_Strafe, out var strafe))
				return;

			caster.StartBuff(BuffId.Ranger_StrapingShot, 0, 0, TimeSpan.FromSeconds(3), caster);
		}

		/// <summary>
		/// Returns the distance to move.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float GetStrafeDistance(ICombatEntity caster, Skill skill)
		{
			var evasion = caster.Properties.GetFloat(PropertyName.DR);
			var level = skill.Level;

			var distance = BaseStrafeDistance + StrafeDistancePerLevel * level;

			distance += StrafeDistanceEvasionMultiplierPerLevel * level * evasion;

			return Math.Min(180, distance);
		}
	}
}
