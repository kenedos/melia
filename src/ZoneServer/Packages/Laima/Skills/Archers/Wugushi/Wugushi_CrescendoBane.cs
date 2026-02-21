using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Handlers.Archers.Wugushi;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handler for the Wugushi skill Crescendo Bane.
	/// Accelerates all poison debuffs on enemies in range, reducing
	/// their tick interval and remaining duration. Also applies a
	/// self-buff that automatically accelerates newly-applied poisons.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wugushi_CrescendoBane)]
	public class Wugushi_CrescendoBaneOverride : IMeleeGroundSkillHandler
	{
		private const float BuffDurationMs = 8000f;
		private const float SplashRadius = 300f;
		private const int MaxTargets = 15;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(50));

			caster.StartBuff(BuffId.Crescendo_Bane_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(BuffDurationMs), caster);

			await skill.Wait(TimeSpan.FromMilliseconds(50));

			var enemiesInRange = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SplashRadius);

			var hasPoisonToPoison = caster.IsAbilityActive(AbilityId.Wugushi25);

			var deadlyVenomMultiplier = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Wugushi35, out var deadlyVenomLevel))
				deadlyVenomMultiplier += deadlyVenomLevel * 0.1f;

			foreach (var target in enemiesInRange.Take(MaxTargets))
			{
				this.AcceleratePoisonDebuffs(target, skill.Level);

				if (hasPoisonToPoison && target.Properties.GetFloat(PropertyName.Attribute) == (float)AttributeType.Poison)
				{
					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier = deadlyVenomMultiplier;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					var hit = new HitInfo(caster, target, skill, skillHitResult);
					hit.ForceId = ForceId.GetNew();
					Send.ZC_HIT_INFO(caster, target, hit);
				}
			}
		}

		private void AcceleratePoisonDebuffs(ICombatEntity target, int skillLevel)
		{
			var buffs = target.Components.Get<BuffComponent>();
			if (buffs == null)
				return;

			var poisonDebuffs = buffs.GetAll(b => b.Data.Tags.HasAny(BuffTag.Poison));

			foreach (var buff in poisonDebuffs)
			{
				Crescendo_Bane_BuffOverride.AccelerateBuff(buff, skillLevel);
			}
		}
	}
}
