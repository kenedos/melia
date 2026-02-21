using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_aklashump_Skill_1)]
	public class Mon_aklashump_Skill_1 : ITargetSkillHandler
	{
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
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 79.884148f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_cleric_devinestigma_hit_smoke_LOW##0.5", 0.5f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.5f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("I_bat009_smoke_re##0.4", 1f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_aklashump_Skill_2)]
	public class Mon_aklashump_Skill_2 : ITargetSkillHandler
	{
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
			await skill.Wait(TimeSpan.FromMilliseconds(850));
			var config = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_explosion001_dark_Low", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("I_explosion002_violet2", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.02f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 60f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			startingPosition = originPos.GetRelative(farPos, distance: 60f);
			endingPosition = originPos.GetRelative(farPos, distance: 60.101402f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			startingPosition = originPos.GetRelative(farPos, distance: 63.922535f);
			endingPosition = originPos.GetRelative(farPos, distance: 58.168789f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			startingPosition = originPos.GetRelative(farPos, distance: 60f);
			endingPosition = originPos.GetRelative(farPos);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_explosion001_dark_Low", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("I_explosion002_violet2", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.02f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}
}
