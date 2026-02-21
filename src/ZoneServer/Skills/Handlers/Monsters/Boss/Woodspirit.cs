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
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_woodspirit_Skill_1)]
	public class Mon_boss_woodspirit_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 100,
				Effect = EffectConfig.None,
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 20f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 10, angle: 30f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 10, angle: -30);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodspirit_Skill_2)]
	public class Mon_boss_woodspirit_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_apple002_mash@#Bip01 L Finger0", 0.7f),
				EndEffect = new EffectConfig("F_explosion048", 0.5f),
				Range = 25f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 2000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 2f),
			};

			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await MissileThrow(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodspirit_Skill_3)]
	public class Mon_boss_woodspirit_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos;
			var endingPosition = originPos.GetRelative(farPos, distance: 231.5899f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1.2f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.5f,
				PositionDelay = 1200f,
				HitEffect = new EffectConfig("None", 0.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodspirit_Skill_4)]
	public class Mon_boss_woodspirit_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_woodspirit_Skill_5)]
	public class Mon_boss_woodspirit_Skill_5 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 0;
			var damageDelay = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var spawnPos = originPos.GetRelative(farPos, distance: 65, angle: 94f);
			MonsterSkillCreateMob(skill, caster, "Onion", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			spawnPos = originPos.GetRelative(farPos, distance: 92, angle: -78f);
			MonsterSkillCreateMob(skill, caster, "Onion", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			spawnPos = originPos.GetRelative(farPos, distance: 64, angle: 161f);
			MonsterSkillCreateMob(skill, caster, "Onion", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			spawnPos = originPos.GetRelative(farPos, distance: 85, angle: 222f);
			MonsterSkillCreateMob(skill, caster, "Onion", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			spawnPos = originPos.GetRelative(farPos, distance: 75, angle: 6f);
			MonsterSkillCreateMob(skill, caster, "Onion", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			spawnPos = originPos.GetRelative(farPos, distance: 77, angle: -22f);
			MonsterSkillCreateMob(skill, caster, "Onion", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}
}
