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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_merregina_Skill_1)]
	public class Mon_boss_merregina_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 5f),
				PositionDelay = 1800,
				Effect = new EffectConfig("F_ground127_water", 1f),
				Range = 50f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_merregina_Skill_2)]
	public class Mon_boss_merregina_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 150);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 4f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup041_water_blue", 1f),
				Range = 50f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1050));
			var targetPos = originPos.GetRelative(farPos, distance: 120, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
			targetPos = originPos.GetRelative(farPos, distance: 60, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 2000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_merregina_Skill_3)]
	public class Mon_boss_merregina_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			var hits = new List<SkillHitInfo>();
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle006_violet", 3f),
				EndEffect = new EffectConfig("F_ground127_water", 0.60000002f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 2f,
				FlyTime = 0.6f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
			};

			for (var i = 0; i < 10; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
				await MissileFall(caster, skill, position, config, hits);

				if (i < 9)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_merregina_Skill_4)]
	public class Mon_boss_merregina_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);

			//SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 4000f, 1, 5, -1, hits);
		}
	}
}
