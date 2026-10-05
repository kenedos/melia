using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Appraiser
{
	/// <summary>
	/// High Scale Magnifying Glass.
	/// Applies a buff to the caster and nearby allies that increases
	/// accuracy and block penetration.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Appraiser_HighMagnifyingGlass)]
	public class Appraiser_HighMagnifyingGlass : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private const float BaseBonusPercent = 14f;
		private const float BonusPercentPerAdditionalLevel = 2f;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(15);
		private static readonly TimeSpan ApplicationDelay = TimeSpan.FromMilliseconds(350);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || caster.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.SetAttackState(true);
			caster.TurnTowards(farPos);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(
				caster,
				caster.Handle,
				originPos,
				caster.Direction,
				Position.Zero
			);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.ApplyBuffs(character, skill));
		}

		private async Task ApplyBuffs(Character caster, Skill skill)
		{
			await skill.Wait(ApplicationDelay);

			var skillLevel = Math.Min(
				Math.Max(skill.Level, 1),
				MaximumSkillLevel
			);

			var bonusPercent =
				BaseBonusPercent +
				(skillLevel - 1) * BonusPercentPerAdditionalLevel;

			bonusPercent *= this.GetEnhanceMultiplier(caster);

			var bonusRate = bonusPercent / 100f;
			var effectRange = skill.Data.SplashRange;

			var allies = caster.Map
				.GetCharacters(character =>
					character != null &&
					!character.IsDead &&
					character.Layer == caster.Layer &&
					!caster.IsEnemy(character) &&
					caster.Position.Get2DDistance(character.Position) <= effectRange)
				.ToList();

			if (!allies.Contains(caster))
				allies.Insert(0, caster);

			foreach (var ally in allies)
			{
				ally.StartBuff(
					BuffId.HighMagnifyingGlass_Buff,
					bonusRate,
					skillLevel,
					BuffDuration,
					caster,
					skill.Id
				);
			}

			caster.SetAttackState(false);
		}

		private float GetEnhanceMultiplier(Character caster)
		{
			var abilityLevel = Math.Min(
				caster.Abilities.GetLevel(AbilityId.Appraiser12),
				MaximumEnhanceLevel
			);

			if (abilityLevel <= 0)
				return 1f;

			var enhancePercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhancePercent += 10f;

			return 1f + enhancePercent / 100f;
		}
	}
}
