using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Extensions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters
{
	[SkillHandler(SkillId.Mon_boss_Achat_Skill_2)]
	public class MonBossAchatSkill2 : ITargetSkillHandler
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
			var position = GetRelativePosition(PosType.Self, caster, distance: 45, angle: 1f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 2500,
				Effect = new EffectConfig("F_rize011", 1.5f),
				Range = 45f,
				KnockdownPower = 100f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Achat_Skill_3)]
	public class MonBossAchatSkill3 : ITargetSkillHandler
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
			var startingPosition = GetRelativePosition(PosType.Self, caster, distance: 30);
			var endingPosition = GetRelativePosition(PosType.Self, caster, distance: 300);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1700f,
				HitEffect = EffectConfig.None,
				Range = 15f,
				KnockdownPower = 100f,
				Delay = 1000f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.04f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Achat_Skill_4)]
	public class MonBossAchatSkill4 : ITargetSkillHandler
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
			var targetPos = farPos;
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 30));
			var hits = new List<SkillHitInfo>();
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_in009_rize##0.5", 0.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_ground028_rize", 1f),
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 10000f, 1, 10, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			hits.Clear();
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 10000f, 1, 10, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			hits.Clear();
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 10000f, 1, 10, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			hits.Clear();
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 10000f, 1, 10, -1, hits);
		}
	}
}
