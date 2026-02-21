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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Moldyhorn_Skill_1)]
	public class Mon_boss_Moldyhorn_Skill_1 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 20);
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_moldyhorn_skl1_1_mash#Bip001 Ponytail1Nub", 0.3f),
				EndEffect = new EffectConfig("F_ground004_green", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 0.5f,
				FlyTime = 0.6f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 2f),
			};

			for (var i = 0; i < 6; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
				await MissileFall(caster, skill, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Moldyhorn_Skill_2)]
	public class Mon_boss_Moldyhorn_Skill_2 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 90);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1300;
			var damageDelay = 1300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Moldyhorn_Skill_3)]
	public class Mon_boss_Moldyhorn_Skill_3 : ITargetSkillHandler
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
			var startingPosition = originPos;
			var endingPosition = caster.Map.Ground.GetLastValidPosition(originPos, originPos.GetRelative(farPos, distance: 30));
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.03f,
				PositionDelay = 1700f,
				HitEffect = new EffectConfig("F_burstup007_smoke", 0.7f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 40f,
			});

			caster.Position = endingPosition;
			Send.ZC_SET_POS(caster, endingPosition);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Moldyhorn_Skill_4)]
	public class Mon_boss_Moldyhorn_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			caster.StartBuff(BuffId.Moldy_skill_buff, 1f, 0f, TimeSpan.FromMilliseconds(20000f), caster);
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Moldyhorn_takedamage2);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Moldyhorn_Skill_5)]
	public class Mon_boss_Moldyhorn_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			caster.StartBuff(BuffId.Moldy_skill_buff, 1f, 0f, TimeSpan.FromMilliseconds(20000f), caster);
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Moldyhorn_takedamage2);
		}
	}
}
