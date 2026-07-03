using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzer Reiter skill Assault Fire / Marching Fire.
	/// SkillId: 51006
	/// ClassName: Schwarzereiter_AssaultFire
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_AssaultFire)]
	public class SchwarzerReiter_AssaultFireOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			var character = caster as Character;

			var hasTakingCover = character != null
				&& character.IsAbilityActive(AbilityId.Schwarzereiter29);

			caster.StartBuff(
				BuffId.AssaultFire_Buff,
				1f,
				hasTakingCover ? 1f : 0f,
				TimeSpan.Zero,
				caster,
				skill.Id);

			// [Arts] Marching Fire: Taking Cover reduces the caster's accuracy while channeling.
			if (hasTakingCover)
			{
				character.Properties.Modify(PropertyName.HR_BM, -1000);
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.HR, PropertyName.HR_BM);
			}
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			var character = caster as Character;

			// Restore the accuracy penalty if Taking Cover was active.
			if (character != null &&
				caster.TryGetBuff(BuffId.AssaultFire_Buff, out var buff) &&
				buff.NumArg2 > 0)
			{
				character.Properties.Modify(PropertyName.HR_BM, 1000);
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.HR, PropertyName.HR_BM);
			}

			caster.StopBuff(BuffId.AssaultFire_Buff);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, originPos, farPos));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			// Cache the character reference.
			var character = caster as Character;

			// Check if [Arts] Marching Fire: Enhanced Upgrade is active.
			// The ability handler itself should only act as a marker.
			var hasEnhancedUpgrade =
				character != null &&
				character.IsAbilityActive(AbilityId.Schwarzereiter24);

			var hitCount = hasEnhancedUpgrade ? 35 : 25;

			var totalDuration = TimeSpan.FromMilliseconds(5000);
			var firstHitDelay = TimeSpan.FromMilliseconds(250);

			var delayBetweenHits = TimeSpan.FromMilliseconds(
				(totalDuration.TotalMilliseconds - firstHitDelay.TotalMilliseconds) / hitCount
			);

			var aniTime = TimeSpan.FromMilliseconds(270);
			var skillHitDelay = TimeSpan.Zero;

			var artsDamageMultiplier = hasEnhancedUpgrade ? 1.3f : 1f;

			await skill.Wait(firstHitDelay);

			for (var i = 0; i < hitCount; i++)
			{
				if (!caster.TryGetBuff(BuffId.AssaultFire_Buff, out var assaultFireBuff))
					break;

				var target = this.GetTarget(caster, farPos);

				if (target != null)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);

					skillHitResult.Damage *= artsDamageMultiplier;

					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, aniTime, skillHitDelay);
					Send.ZC_SKILL_HIT_INFO(caster, skillHit);
				}

				if (i + 1 < hitCount)
					await skill.Wait(delayBetweenHits);
			}

			caster.SetAttackState(false);
		}

		private ICombatEntity GetTarget(ICombatEntity caster, Position farPos)
		{
			var splashRadius = 180;
			var splashArea = new Circle(farPos, splashRadius);

			return caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => target.Position.Get2DDistance(farPos))
				.FirstOrDefault();
		}
	}
}
