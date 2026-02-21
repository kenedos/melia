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
	[SkillHandler(SkillId.Mon_G_Tower_5_Skill_1)]
	public class Mon_G_Tower_5_Skill_1 : ITargetSkillHandler
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
			var angles = new float[] { 0f, 30f, 60f, 90f, 120f, 149f, 180f, 209f, 240f, 270f, 299f, 329f, 360f };

			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1.8f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_ground045_smoke", 1f),
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			foreach (var angle in angles)
			{
				var position = originPos.GetRelative(farPos, distance: 200, angle: angle);
				await EffectAndHit(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_G_Tower_5_Skill_2)]
	public class Mon_G_Tower_5_Skill_2 : ITargetSkillHandler
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
			var angles = new float[] { 0f, 30f, 60f, 90f, 120f, 149f, 180f, 209f, 240f, 270f, 299f, 329f, 360f };

			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1.4f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_ground045_smoke", 1f),
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			foreach (var angle in angles)
			{
				var position = originPos.GetRelative(farPos, distance: 130, angle: angle);
				await EffectAndHit(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_G_Tower_5_Skill_3)]
	public class Mon_G_Tower_5_Skill_3 : ITargetSkillHandler
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
			var angles = new float[] { 0f, 30f, 60f, 90f, 120f, 149f, 180f, 209f, 240f, 270f, 299f, 329f, 360f };

			var innerConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_ground045_smoke", 0.6f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			foreach (var angle in angles)
			{
				var position = originPos.GetRelative(farPos, distance: 100, angle: angle);
				await EffectAndHit(skill, caster, position, innerConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1));

			var outerConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 2f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_ground045_smoke", 1.2f),
				Range = 65f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var outerAngles = new float[] { 360f, 329f, 299f, 270f, 240f, 209f, 180f, 149f, 90f, 120f, 60f, 30f, 0f };

			foreach (var angle in outerAngles)
			{
				var position = originPos.GetRelative(farPos, distance: 250, angle: angle);
				await EffectAndHit(skill, caster, position, outerConfig);
			}
		}
	}

}
