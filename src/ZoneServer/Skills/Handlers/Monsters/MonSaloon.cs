using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Saloon_Skill_1)]
	public class Mon_Saloon_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(300);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 0;
			var damageDelay = 300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			// TODO: No Implementation S_R_COND_OR_START

			if (caster.IsOwnerAbilityActive(AbilityId.Sorcerer5))
			{
				var casterINT = caster.Properties.GetFloat(PropertyName.INT);
				if (caster.TryGetOwner(out var owner))
					casterINT = owner.Properties.GetFloat(PropertyName.INT);
				SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, casterINT, 10000f, 1, 2 * owner.GetAbilityLevel(AbilityId.Sorcerer5), -1, hits);
			}
			if (caster.IsOwnerAbilityActive(AbilityId.Sorcerer6))
			{
				if (caster.TryGetOwner(out var owner))
					SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 10000f, 1, 2 * owner.GetAbilityLevel(AbilityId.Sorcerer6), -1, hits);
			}
			// TODO: No Implementation S_R_COND_OR_END

		}
	}

	[SkillHandler(SkillId.Mon_Saloon_Skill_2)]
	public class Mon_Saloon_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(300);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 0;
			var damageDelay = 300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			// TODO: No Implementation S_R_COND_OR_START

			if (caster.IsOwnerAbilityActive(AbilityId.Sorcerer5))
			{
				var casterINT = caster.Properties.GetFloat(PropertyName.INT);
				if (caster.TryGetOwner(out var owner))
					casterINT = owner.Properties.GetFloat(PropertyName.INT);
				SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, casterINT, 10000f, 1, 2 * owner.GetAbilityLevel(AbilityId.Sorcerer5), -1, hits);
			}
			if (caster.IsOwnerAbilityActive(AbilityId.Sorcerer6))
			{
				if (caster.TryGetOwner(out var owner))
					SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 10000f, 1, 2 * owner.GetAbilityLevel(AbilityId.Sorcerer6), -1, hits);
			}
			// TODO: No Implementation S_R_COND_OR_END

		}
	}

	[SkillHandler(SkillId.Mon_Saloon_Skill_3)]
	public class Mon_Saloon_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(300);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 0;
			var damageDelay = 300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			// TODO: No Implementation S_R_COND_OR_START

			if (caster.IsOwnerAbilityActive(AbilityId.Sorcerer5))
			{
				var casterINT = caster.Properties.GetFloat(PropertyName.INT);
				if (caster.TryGetOwner(out var owner))
					casterINT = owner.Properties.GetFloat(PropertyName.INT);
				SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, casterINT, 10000f, 1, 2 * owner.GetAbilityLevel(AbilityId.Sorcerer5), -1, hits);
			}
			if (caster.IsOwnerAbilityActive(AbilityId.Sorcerer6))
			{
				if (caster.TryGetOwner(out var owner))
					SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 10000f, 1, 2 * owner.GetAbilityLevel(AbilityId.Sorcerer6), -1, hits);
			}
			// TODO: No Implementation S_R_COND_OR_END

		}
	}

}
