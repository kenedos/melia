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
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Yggdrasil.Util;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_lepus_Skill_1)]
	public class Mon_boss_lepus_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1000);
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 30, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_lepus_Skill_2)]
	public class Mon_boss_lepus_Skill_2 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_wizard_icebolt_force_ice#Bip001 L Finger0Nub", 3.5f),
				EndEffect = new EffectConfig("F_explosion080_ice", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 24f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			var position = originPos.GetRelative(farPos, distance: 80, angle: 25f);
			await MissileThrow(skill, caster, position, missileConfig);
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_wizard_icebolt_force_ice#Bip001 R Finger0Nub", 3.5f),
				EndEffect = new EffectConfig("F_explosion080_ice", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 24f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			position = originPos.GetRelative(farPos, distance: 138, angle: 6f);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = originPos.GetRelative(farPos, distance: 100);
			await MissileThrow(skill, caster, position, missileConfig);
			position = originPos.GetRelative(farPos, distance: 110, angle: -37f);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = originPos.GetRelative(farPos, distance: 40, angle: -39f);
			await MissileThrow(skill, caster, position, missileConfig);
			position = originPos.GetRelative(farPos, distance: 150, angle: -22f);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			position = originPos.GetRelative(farPos, distance: 40, angle: -39f);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_lepus, 0f, RandomProvider.Get().Next(100, 301), (int)RandomProvider.Get().Next(3, 6), RandomProvider.Get().Next(40, 81), 250f, 50f, 0f);
			position = originPos.GetRelative(farPos, distance: 138, angle: 6f);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_lepus, 0.34906584f, RandomProvider.Get().Next(100, 301), (int)RandomProvider.Get().Next(3, 6), RandomProvider.Get().Next(40, 81), 250f, 50f, 0f);
			position = originPos.GetRelative(farPos, distance: 80, angle: 25f);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_lepus, 0.69813168f, RandomProvider.Get().Next(100, 301), (int)RandomProvider.Get().Next(3, 6), RandomProvider.Get().Next(40, 81), 250f, 50f, 0f);
			position = originPos.GetRelative(farPos, distance: 100);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_lepus, 1.0471976f, RandomProvider.Get().Next(100, 301), (int)RandomProvider.Get().Next(3, 6), RandomProvider.Get().Next(40, 81), 250f, 50f, 0f);
			position = originPos.GetRelative(farPos, distance: 110, angle: -37f);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_lepus, 1.3962634f, RandomProvider.Get().Next(100, 301), (int)RandomProvider.Get().Next(3, 6), RandomProvider.Get().Next(40, 81), 250f, 50f, 0f);
			position = originPos.GetRelative(farPos, distance: 150, angle: -22f);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_lepus, 1.7453293f, RandomProvider.Get().Next(100, 301), (int)RandomProvider.Get().Next(3, 6), RandomProvider.Get().Next(40, 81), 250f, 50f, 0f);
		}
	}

	[SkillHandler(SkillId.Mon_boss_lepus_Skill_3)]
	public class Mon_boss_lepus_Skill_3 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}
}
