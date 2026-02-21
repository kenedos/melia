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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Lapene_Skill_1)]
	public class Mon_boss_Lapene_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 35f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Lapene_Skill_2)]
	public class Mon_boss_Lapene_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1200));

			var padSequence = new[]
			{
				PadName.Lapene_Donuts,
				PadName.Lapene_Donuts1,
				PadName.Lapene_Donuts2,
				PadName.Lapene_Donuts3,
				PadName.Lapene_Donuts3,
				PadName.Lapene_Donuts2,
				PadName.Lapene_Donuts1,
				PadName.Lapene_Donuts,
			};

			foreach (var padName in padSequence)
			{
				var targetPos = originPos.GetRelative(farPos);
				SkillCreatePad(caster, skill, targetPos, 0f, padName);
				await skill.Wait(TimeSpan.FromMilliseconds(350));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Lapene_Skill_3)]
	public class Mon_boss_Lapene_Skill_3 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(800));

			var randomHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground140_orange_circle", 0.4f),
				PositionDelay = 1000,
				Effect = new EffectConfig("I_Lapene_skl2_gasi_mash", 0.6f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			var relativeHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground140_orange_circle", 0.4f),
				PositionDelay = 500,
				Effect = new EffectConfig("I_Lapene_skl2_gasi_mash", 0.6f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			var randomDelays = new[] { 250, 50, 150, 150, 150, 150 };

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, randomHitConfig);

			foreach (var delay in randomDelays)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delay));
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
				await EffectAndHit(skill, caster, position, randomHitConfig);
			}

			position = originPos.GetRelative(farPos, distance: 40, angle: 180f, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);
			position = originPos.GetRelative(farPos, distance: 35, angle: -90f, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);
			position = originPos.GetRelative(farPos, distance: 35, angle: -45f, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);
			position = originPos.GetRelative(farPos, distance: 30, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);
			position = originPos.GetRelative(farPos, distance: 40, angle: 90f, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);
			position = originPos.GetRelative(farPos, distance: 35, angle: 35f, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);
			position = originPos.GetRelative(farPos, distance: 40, angle: 135f, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);
			position = originPos.GetRelative(farPos, distance: 40, angle: -135f, height: 1);
			await EffectAndHit(skill, caster, position, relativeHitConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, randomHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, randomHitConfig);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Lapene_Skill_4)]
	public class Mon_boss_Lapene_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 60, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 3000;
			var damageDelay = 3200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 45f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 2300;
			damageDelay = 5500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Lapene_Skill_5)]
	public class Mon_boss_Lapene_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(800));

			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground140_orange_circle##1", 0.4f),
				PositionDelay = 1000,
				Effect = new EffectConfig("I_Lapene_skl2_gasi_red_mash", 0.6f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts1);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts3);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await EffectAndHit(skill, caster, position, hitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts3);
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts2);
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts1);
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Lapene_Donuts);
		}
	}
}
